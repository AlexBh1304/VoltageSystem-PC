using Avalonia.Controls;
using System.IO;
using Microsoft.Data.Sqlite;
using Avalonia.Interactivity;
using System;
using System.IO.Ports;
using PrimeraApp.Services;
using PrimeraApp.Models;
using Avalonia.Threading;
using PrimeraApp.Hardware;
using System.Collections.Generic;
namespace PrimeraApp;

public partial class VoltageWindow : Window
{
    private DispatcherTimer _timer;
    private double voltaje;
    private bool IsRunning = false;
    private bool IsConnected = false;
    private List<double> list =  new List<double>();
    private string[] SourceLed = ["Red","Yellow","Turquoise","Pink","Green"];
    private int lastSeq = -1;
    private int lostFrames= 0;

    private SerialPortDriverxd sp = new SerialPortDriverxd();
    public VoltageWindow()
    {

        InitializeComponent();
        
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(300);
        _timer.Tick += ActualizarVoltaje;
        
        UsernameLabel.Content = $"User: {CurrentUser.Data.Name}";

        PortComboBox.ItemsSource = SerialPort.GetPortNames();     
        SourceComboBox.ItemsSource = SourceLed;
        UpdateDg();
        UsernameLabel.Content = $"User: {CurrentUser.Data.Name}";       
    }

    private void ActualizarVoltaje(object? sender, EventArgs e)
    {
        string line = sp.ReadLine();
        if(line == "")
        {
            return;
        }
        
        Frame frame = FrameParser.CheckFrame(line);

        if(frame.Status)
        {

            if(lastSeq != -1)
            {
                int expected = (lastSeq + 1) % 65536;
                int gap = (frame.Seq - expected + 65336) % 65336;
                lostFrames += gap;
            }

            lastSeq = frame.Seq;
            LostFramesLbl.Content = $"Lost Frames: {lostFrames}";

            voltaje = frame.Voltage;
            VoltageLbl.Content = $"{voltaje}V";
            if(IsRunning)
            {
                list.Add(frame.Voltage);
                
                RunStatusLabel.Content = $"Measurement count: {list.Count} / 50";
                if(list.Count == 50)
                {
                    DatabaseService.SaveRun(list, SourceComboBox.SelectedIndex);
                    IsRunning = false;
                    UpdateDg();
                    RunStatusLabel.Content = "Run completed!";
                    lastSeq = -1;
                    lostFrames= 0;
                }
            }
            
        }

        else
        {
            CurrentVoltageLabel.Content = $"Error: {frame.Error}";
        }
    }

    private void Run_Click(object? sender, RoutedEventArgs e)
    {
        if((!IsRunning) && IsConnected && SourceComboBox.SelectedIndex!=-1)
        {
            IsRunning = true;
            list.Clear();
            RunStatusLabel.Content = "Run in progress";
            Console.WriteLine("Runclickdetected");

        }
    }

    private void Export_Click(object? sender, RoutedEventArgs e)
    {
        using var connection = new SqliteConnection("Data Source=database.db");
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM RunData";

        using var reader = cmd.ExecuteReader();
        using var writer = new StreamWriter("RunDataInfo.csv");

        for(int i = 0; i < reader.FieldCount; i++)
        {
            writer.Write(reader.GetName(i));
            if(i < reader.FieldCount - 1)
            {
                writer.Write(",");
            }
        }
        
        writer.WriteLine();

        
        while(reader.Read())
        {
            for(int i = 0; i < reader.FieldCount; i++)
            {
                writer.Write(reader.GetValue(i));

                if( i < reader.FieldCount - 1)
                {
                    writer.Write(",");
                }

            }

            writer.WriteLine();
                
               
        }


    }
    private void UpdateDg()
    {
         using var connection = new SqliteConnection("Data Source=database.db");
        using var command = connection.CreateCommand();
        connection.Open();
        command.CommandText= """
            SELECT RunId, SourceId, Voltage, TimeStamp
            FROM RunData
        """;
        using var reader = command.ExecuteReader();
        
        List<Measurement> lista = new List<Measurement>();


        while(reader.Read())
        {
            lista.Add(new Measurement(){id = reader.GetInt32(0), sourceid = reader.GetInt32(1), voltage = reader.GetDouble(2),timestamp = reader.GetString(3)});

        }
        
        DataGridLog.ItemsSource = lista;

    }
    private void Connect(object? sender, RoutedEventArgs e)
    {
        if(((string?)PortComboBox.SelectedItem) != null)
        {
            try
            {
                sp.Open((string)PortComboBox.SelectedItem, 9600); 
                _timer.Start();
                DeviceStatusText.Content = $"Device status: Connected";
                IsConnected = true;  
                Console.WriteLine("Click connect detected");
            }
            catch
            {
                DeviceStatusText.Content = $"Device status: Error while opening the port";
            }
        }
    }

}

