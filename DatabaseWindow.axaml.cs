
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.IO.Ports;

namespace PrimeraApp;

public partial class DatabaseWindow : Window
{
    public DatabaseWindow()
    {
        InitializeComponent();
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


