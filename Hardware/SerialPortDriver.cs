using System;
using System.IO.Ports;

namespace VoltageSystem.Hardware;
public class SerialPortDriverxd
{
    private SerialPort _port = new SerialPort();

    public void Open(string portName, int baudRate)
    {
        if(!_port.IsOpen)
        {
            _port.PortName = portName;
            _port.BaudRate = baudRate;
            _port.NewLine = "\0";
            _port.ReadTimeout = 200;
            _port.Open();
        }
    }

    public void Close()
    {
        if(_port.IsOpen)
        {
            _port.Close();
        }
    }

    public string ReadLine()
    {
        if(_port.BytesToRead == 0)
        {
            return "";
        }

        try
        {
            return _port.ReadLine();
        }
        catch(TimeoutException)
        {
            return "";
        }
    }
}


 public static class SerialPortDriver
 {
 
     static private SerialPort _serialport = new SerialPort();
 
    public static byte[] RxByte()
    {
        byte[] buffer = new byte[2];
        _serialport.Read(buffer, 0, 2);
        return buffer;
    }
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
         _serialport.Close();
     }
}
