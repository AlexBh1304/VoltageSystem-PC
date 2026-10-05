using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.IO.Ports;

namespace PrimeraApp;

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


public static class SerialPortDriver
{
    

    static private SerialPort _serialport = new SerialPort();

//    public static byte[] RxByte()
//    {
//        byte[] buffer = new byte[2];
//        _serialport.Read(buffer, 0, 2);
//        return buffer;
//    }
    public static byte[] RxBytes(int cantidad)
    {
        byte[] buffer = new byte[cantidad];

        int offset = 0;

        while (offset < cantidad)
        {
            offset += _serialport.Read(buffer, offset,cantidad - offset);
        }

        return buffer;
    }
    public static void TxByte(byte dato)
    {
        _serialport.Write(new byte[] {dato}, 0, 1);
    }

    public static void OpenPort()
    {
        _serialport.PortName = "/dev/ttyUSB0";
        _serialport.BaudRate = 9600;
        _serialport.Open();
    }

    public static void ClosePort()
    {

    }
}
