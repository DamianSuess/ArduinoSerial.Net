using System;

namespace DesktopComms
{
  public class Program
  {
    public static void Main(string[] args)
    {
      Helpers.DiscoverPorts();

      Console.Write("Input COM '##' or 'q' to exit: ");
      var cmd = Console.ReadLine();

      if (cmd == "q")
        return;

      if (!Int32.TryParse(cmd, out int portNum))
      {
        Console.WriteLine("Invalid COM Port number. Exiting");
        return;
      }

      SerialReadWrite.Handle(portNum);

      // ProgramSimple.Handle(portNum);
      // ProgramThread.Handle(portNum);
      // ProgramEventHandler.Handle(portNum);
    }
  }
}