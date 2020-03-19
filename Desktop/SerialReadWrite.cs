using System;
using System.IO.Ports;

namespace DesktopComms
{
  public class SerialReadWrite
  {
    private static SerialPort _serialPort;

    public static void Handle(int portNum)
    {

      string comPort = "COM" + portNum;
      SerialInit(comPort);

      // Data Received Event Handler
      _serialPort.DataReceived += SerialPort_DataReceived;

      Console.WriteLine("Listening...");
      _serialPort.Open();

      Console.WriteLine("Press any key to quit.");
      Console.ReadKey();

      _serialPort.Close();
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

    private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      SerialPort port = (SerialPort)sender;
      
      string data = port.ReadExisting();
      Console.WriteLine($"> '{data}' - '{Helpers.ConvertStringToHex(data)}'");

      ParseInput(port, data);
    }

    private static void ParseInput(SerialPort port, string data)
    {
      string input = data.Substring(0, 1);

      string response = string.Empty;

      switch (input)
      {
        case "0":
          response = "1";
          break;
        case "A":
          response = "2";
          break;
        case "B":
          response = "3";
          break;
        case "C":
          // Start over again
          response = "4";
          break;

        default:
          // invalid command
          response = "1";
          break;
      }

      port.Write(response);
    }
  }
}