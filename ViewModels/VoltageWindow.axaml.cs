using Avalonia.Controls;
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
    private List<double> list =  new List<double>();

    private SerialPortDriverxd sp = new SerialPortDriverxd();
    public VoltageWindow()
    {

        InitializeComponent();
        //SerialPortDriver.OpenPort();
        
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(300);
        _timer.Tick += ActualizarVoltaje;
        
        UsernameLabel.Content = $"User: {CurrentUser.Data.Name}";

        PortComboBox.ItemsSource = SerialPort.GetPortNames();         
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
            voltaje = frame.Voltage;
            CurrentVoltageLabel.Content = $"Current Voltage: {voltaje}V";
            if(IsRunning)
            {
                list.Add(frame.Voltage);
                
                RunStatusLabel.Content = $"Run completed! Count:{list.Count}";
                if(list.Count == 50)
                {
                    DatabaseService.SaveRun(list,0);
                    IsRunning = false;
                    RunStatusLabel.Content = "Run completed!";
                }
            }
            
        }

        else
        {
            CurrentVoltageLabel.Content = $"Error: {frame.Error}";
        }
        //voltaje = ADC.TomarLectura();
    }

    private void MiBoton_Click(object? sender, RoutedEventArgs e)
    {
        Saludar();
    }

    private void Run_Click(object? sender, RoutedEventArgs e)
    {
        IsRunning = true;
        list.Clear();
        RunStatusLabel.Content = "Run in progress";
        Console.WriteLine("Runclickdetected");
    }


    private void Saludar()
    {
        DatabaseWindow databasewindow = new DatabaseWindow();
        databasewindow.Show();

        SerialPortDriver.ClosePort();
        this.Close();
    }

    private void GuardarRegistro_Click(object? sender, RoutedEventArgs e)
    {
        using var connection = new SqliteConnection("Data Source=database.db");
        using var command = connection.CreateCommand();
        connection.Open();
        command.CommandText= """
            INSERT INTO Measurements(Voltage, Timestamp, User)
            VALUES ($voltage, $timestamp, $user);
        """;
        command.Parameters.AddWithValue("$voltage", voltaje);
        command.Parameters.AddWithValue("$timestamp", DateTime.Now.ToString());
        command.Parameters.AddWithValue("$user", "fulanito");
        command.ExecuteNonQuery();
         
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
                Console.WriteLine("Click connect detected");
            }
            catch
            {
                DeviceStatusText.Content = $"Device status: Error while opening the port";
            }
        }
    }

}


//static class ADC
//{
//    public static int Valor {get; private set;}
//    
//
//    public static int TomarLectura()
//    {
//        byte[] datos  = SerialPortDriver.RxBytes(2);
//        Valor = datos[0] | (datos[1] << 8);
//        return Valor;
//    }
//    
//}
//
//
//public static class SerialPortDriver
//{
//    
//
//    static private SerialPort _serialport = new SerialPort();
//
////    public static byte[] RxByte()
////    {
////        byte[] buffer = new byte[2];
////        _serialport.Read(buffer, 0, 2);
////        return buffer;
////    }
//    public static byte[] RxBytes(int cantidad)
//    {
//        byte[] buffer = new byte[cantidad];
//
//        int offset = 0;
//
//        while (offset < cantidad)
//        {
//            offset += _serialport.Read(buffer, offset,cantidad - offset);
//        }
//
//        return buffer;
//    }
//    public static void TxByte(byte dato)
//    {
//        _serialport.Write(new byte[] {dato}, 0, 1);
//    }
//
//    public static void OpenPort()
//    {
//        _serialport.PortName = "/dev/ttyUSB0";
//        _serialport.BaudRate = 9600;
//        _serialport.Open();
//    }
//
//    public static void ClosePort()
//    {
//
//    }
//
