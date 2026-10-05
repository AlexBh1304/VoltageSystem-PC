using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.IO.Ports;

namespace PrimeraApp;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void MiBoton_Click(object? sender, RoutedEventArgs e)
    {
        Saludar();
    }

    private void Saludar()
    {
 ////       TextBox1.Text = Convert.ToString(ADC.TomarLectura());
    }
}


