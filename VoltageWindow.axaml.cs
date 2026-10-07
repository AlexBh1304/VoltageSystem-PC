using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.IO.Ports;

namespace PrimeraApp;

public partial class VoltageWindow : Window
{
    public VoltageWindow()
    {
        InitializeComponent();
    }

    private void MiBoton_Click(object? sender, RoutedEventArgs e)
    {
        Saludar();
    }

    private void Saludar()
    {
        DatabaseWindow databasewindow = new DatabaseWindow();
        databasewindow.Show();
        this.Close();
    }
}


