
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.IO.Ports;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
namespace PrimeraApp;

public partial class DatabaseWindow : Window
{
    public DatabaseWindow()
    {
        InitializeComponent();
        using var connection = new SqliteConnection("Data Source=database.db");
        using var command = connection.CreateCommand();
        connection.Open();
        command.CommandText= """
            SELECT Id, Voltage, Timestamp, User
            FROM Measurements
        """;
        using var reader = command.ExecuteReader();
        
        List<Measurement> lista = new List<Measurement>();


        while(reader.Read())
        {
            lista.Add(new Measurement(){id = reader.GetInt32(0), voltage = reader.GetInt32(1),datestamp = reader.GetString(2), user = reader.GetString(3)});

        }
        
        DataGridLog.ItemsSource = lista;
        

    }

    private void MiBoton_Click(object? sender, RoutedEventArgs e)
    {
        Saludar();
    }

    private void Saludar()
    {
        VoltageWindow voltagewindow = new VoltageWindow();
        voltagewindow.Show();
        this.Close();
    }   
    
    private void Closear(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }   



}

public class Measurement
{
    public int id {get; set;}
    public int voltage{get; set;}
    public string datestamp{get; set;}
    public string user{get; set;}

}

