using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.IO.Ports;
using Avalonia.Threading;
namespace PrimeraApp;

public partial class VoltageWindow : Window
{
    private DispatcherTimer _timer;

    public VoltageWindow()
    {

        InitializeComponent();
        SerialPortDriver.OpenPort();

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(300);
        _timer.Tick += ActualizarVoltaje;
        _timer.Start();
        

    }

    private void ActualizarVoltaje(object? sender, EventArgs e)
    {
        double voltaje = ADC.TomarLectura();
        CurrentVoltageLabel.Content = $"Current Voltage: {voltaje}V";
    }

    private void MiBoton_Click(object? sender, RoutedEventArgs e)
    {
        Saludar();
    }

    private void GuardarRegistro_Click(object? sender, RoutedEventArgs e)
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
//}
