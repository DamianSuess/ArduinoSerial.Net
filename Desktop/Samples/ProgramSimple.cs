using System;
using System.IO.Ports;
using System.Threading;

namespace DesktopComms
{
  public class ProgramSimple
  {
    private static SerialPort _serialPort;

    public static void Handle(int portNum)
    {
      Console.WriteLine("Simple loop-read example");

      string port = "COM" + portNum;
      ReadSerialPort(port);
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
      _serialPort.RtsEnable = true;
    }

    private static void SerialOpen(bool dataTerminalReady = true)
    {
      _serialPort.Open();
      if (dataTerminalReady)
      {
        _serialPort.DtrEnable = true;
        _serialPort.DiscardInBuffer();
        Thread.Sleep(500);
        _serialPort.DtrEnable = false;
      }
    }

    private static void ReadSerialPort(string comPort)
    {
      Console.WriteLine("Listening...");

      SerialInit(comPort);
      SerialOpen(true);

      int cnt = 0;
      while (true)
      {
        cnt++;
        if (SerialRead(out string read))
        {
          // Output connection read
          Console.WriteLine($"{cnt}: '{read}'");
        }
        else
        {
          // Attempt to reconnect
          Console.WriteLine("Attempting reconnect...");
          SerialOpen(true);
        }

        Thread.Sleep(400);
      }

      _serialPort.Dispose();
    }

    private static bool SerialRead(out string data)
    {
      // Old method, not working with Gemma M0
      // _serialPort.ReadExisting();
      try
      {
        int bytesToRead = _serialPort.BytesToRead;

        if (bytesToRead > 0)
        {
          //byte[] buffer = new byte[bytesToRead];
          //_serialPort.Read(buffer, 0, bytesToRead);
          //data = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

          data = _serialPort.ReadLine();

          //// _serialPort.DiscardInBuffer();
        }
        else
          data = string.Empty;
      }
      catch (Exception)
      {
        data = string.Empty;
        return false;
      }

      return true;
    }
  }
}