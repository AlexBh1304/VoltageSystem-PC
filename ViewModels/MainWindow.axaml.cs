using VoltageSystem.Hardware;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.IO.Ports;

namespace VoltageSystem;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        SerialPortDriver.OpenPort();
        InitializeComponent();
        

    }

    private void MiBoton_Click(object? sender, RoutedEventArgs e)
    {
        Saludar();
    }

    private void Saludar()
    {
        TextBox1.Text = Convert.ToString(ADC.TomarLectura());
    }
}



//namespace VoltageSystem;

static class ADC
{
    public static int Valor {get; private set;}

    public static double TomarLectura()
    {
        int datotx = 1;
        double voltage;
        SerialPortDriver.TxByte((byte)datotx);
        byte[] datos  = SerialPortDriver.RxBytes(2);
        Valor = datos[0] | (datos[1] << 8);
        voltage = (Valor * 3.3) / 1023;
        voltage = Math.Round(voltage,4);
        return voltage;
    }
    
}



