using System;
using System.Management;
using System.Text;

namespace DesktopComms
{
  public static class Helpers
  {
    public static void DiscoverPorts()
    {
      ManagementScope scope = new ManagementScope();
      SelectQuery query = new SelectQuery("SELECT * FROM Win32_SerialPort");
      ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
      Console.WriteLine("Available COM Ports:");

      try
      {
        foreach (var item in searcher.Get())
        {
          var desc = item["Description"].ToString();
          var devId = item["DeviceID"].ToString();

          Console.WriteLine($"- DevId: '{devId}' - Description: '{desc}'");

          if (desc.Contains("Arduino"))
          {
            Console.WriteLine($"- Arduino found on Port: {devId}");
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Exceptions: \r\n\t" + ex.Message);
      }

      Console.WriteLine("");
    }

    public static string ConvertStringToHex(string data, bool removeDash = false, bool isUnicode = false)
    {
      var hexString = string.Empty;

      if (!isUnicode)
      {
        byte[] bytes = Encoding.Default.GetBytes(data);
        hexString = BitConverter.ToString(bytes);
      }
      else
      {
        // Unicode strings to hex
        // Example: data = "0𝟏𝟚𝟥𝟰𝟻";

        var sb = new StringBuilder();

        var bytes = Encoding.Unicode.GetBytes(data);
        foreach (var t in bytes)
        {
          sb.Append(t.ToString("X2"));
        }
        hexString = sb.ToString();

        ////// left padded with 0 - "0030d835dfcfd835dfdad835dfe5d835dff0d835dffb"
        ////var s1 = string.Concat(data.Select(c => $"{(int)c:x4}"));
        ////
        ////var sL = BitConverter.ToString(Encoding.Unicode.GetBytes(data)).Replace("-", "");       // Little Endian "300035D8CFDF35D8DADF35D8E5DF35D8F0DF35D8FBDF"
        ////var sB = BitConverter.ToString(Encoding.BigEndianUnicode.GetBytes(data)).Replace("-", ""); // Big Endian "0030D835DFCFD835DFDAD835DFE5D835DFF0D835DFFB"
        ////
        ////// no Encodding "300035D8CFDF35D8DADF35D8E5DF35D8F0DF35D8FBDF"
        ////byte[] b = new byte[data.Length * sizeof(char)];
        ////Buffer.BlockCopy(data.ToCharArray(), 0, b, 0, b.Length);
        ////hexString = BitConverter.ToString(b);
      }

      if (removeDash)
        hexString = hexString.Replace("-", "");
      return hexString;
    }

    public static string ConvertHexToString(string hexString)
    {
      var bytes = new byte[hexString.Length / 2];
      for (var i = 0; i < bytes.Length; i++)
      {
        bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
      }

      return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
    }
  }
}