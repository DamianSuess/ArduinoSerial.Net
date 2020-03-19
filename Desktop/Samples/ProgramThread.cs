using System;
using System.IO.Ports;
using System.Threading;

namespace DesktopComms
{
  public class ProgramThread
  {
    private static SerialPort _serialPort;
    private static bool _cancelFlag = false;

    public static void Handle(int portNum)
    {
      Console.WriteLine("Thread example.");

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
    }

    private static void SerialReadStream(string comPort)
    {
      // https://docs.microsoft.com/en-us/dotnet/api/system.io.ports.serialport.readline?view=netframework-4.8
      Console.WriteLine("Listening...");
      Thread readThread = new Thread(Thread_Read);
      string userInput = "";

      SerialInit(comPort);

      _serialPort.Open();
      ////bool dataTerminalReady = true;
      ////if (dataTerminalReady)
      ////{
      ////  _serialPort.DtrEnable = true;
      ////  _serialPort.DiscardInBuffer();
      ////  Thread.Sleep(500);
      ////  _serialPort.DtrEnable = false;
      ////}

      readThread.Start();

      while (!_cancelFlag)
      {
        userInput = Console.ReadLine();
        if (userInput.Equals("q", StringComparison.OrdinalIgnoreCase))
        {
          _cancelFlag = true;
        }
      }

      readThread.Join();
      _serialPort.Close();
    }

    private static void Thread_Read()
    {
      int cnt = 0;

      while (!_cancelFlag)
      {
        cnt++;
        try
        {
          string message = _serialPort.ReadLine();
          Console.WriteLine($"{cnt}: '{message}'");
        }
        catch (TimeoutException ex)
        {
          Console.WriteLine($"EX: {ex.Message} - {ex.HelpLink}");
        }
      }
    }
  }
}