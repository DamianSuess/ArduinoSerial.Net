using System;
using System.IO.Ports;

namespace DesktopComms
{
  public class ProgramEventhandler
  {
    private static SerialPort _serialPort;

    public static void Handle(int portNum)
    {
      string port = "COM" + portNum;
      SerialReadStream(port);
    }

    private static void SerialInit(string portName)
    {
      // https://github.com/egorgrushko/SerialMonitor/blob/master/Serial%20Monitor/SerialMonitorControl.xaml.cs

      if (_serialPort != null)
        _serialPort.Dispose();

      _serialPort = new SerialPort();
      _serialPort.PortName = portName;
      _serialPort.BaudRate = 9600;
      _serialPort.DataBits = 8;
      _serialPort.Handshake = Handshake.None;
      _serialPort.Parity = Parity.None;
      _serialPort.StopBits = StopBits.One;

      _serialPort.ReadTimeout = 500;
      _serialPort.WriteTimeout = 500;

      // Arduino Gemma M0 requires Request to Send to be TRUE
      _serialPort.RtsEnable = true; // Request to send
      _serialPort.DtrEnable = true; // Data Termianl Ready
    }

    private static void SerialReadStream(string comPort)
    {
      // https://docs.microsoft.com/en-us/dotnet/api/system.io.ports.serialport.readline?view=netframework-4.8
      Console.WriteLine("Listening...");

      SerialInit(comPort);

      // Data Received Event Handler
      _serialPort.DataReceived += _serialPort_DataReceived;

      _serialPort.Open();

      Console.WriteLine("Press any key to quit.");
      Console.ReadKey();

      _serialPort.Close();
    }

    private static void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      SerialPort port = (SerialPort)sender;
      string data = port.ReadExisting();

      //Console.WriteLine($"> '{data}'");
      Console.WriteLine($"> '{Helpers.ConvertStringToHex(data)}'");
    }
  }
}