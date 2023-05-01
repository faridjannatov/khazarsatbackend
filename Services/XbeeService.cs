using System.IO.Ports;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CansatGround.Models;
using Microsoft.AspNetCore.SignalR;

namespace CansatGround.Services;

public class XbeeService
{
    private string _buffer = String.Empty;
    private IConfiguration _configuration { get; set; }
    public SerialPort _xbeeSerial;
    private CSVService _csvService;
    public string lastTelemetryData { get; set; }

    public XbeeService(IConfiguration configuration, CSVService csvService)
    {
        _xbeeSerial = new SerialPort();
        _configuration = configuration;
        _csvService = csvService;
    }

    public CansatGroundProperties ReadConfig()
    {
        var cansatGroundProperties = _configuration.To<CansatGroundProperties>("CansatGroundProperties");
        return cansatGroundProperties;
    }

    public void ClosePort(string port)
    {
        _xbeeSerial.Close();
    }

    public void RunSerialPort(string port)
    {
        var xConfig = ReadConfig().XbeeProperties;
        _xbeeSerial.BaudRate = xConfig.BaudRate;
        _xbeeSerial.PortName = port;
        _xbeeSerial.Parity = Parity.None;
        _xbeeSerial.Handshake = Handshake.None;
        _xbeeSerial.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);

        try
        {
            _xbeeSerial.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
    }

    public void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;

        try
        {
            string xbeeMessage = sp.ReadExisting();
            SerialPrint(xbeeMessage, sp);
//            string xbeeMessage11 = sp.ReadLine();
         //   Console.WriteLine("readline::   "+ xbeeMessage11);
            string xbeeMessageTrim = xbeeMessage.Trim();    
            string[] dataArray = xbeeMessageTrim.Split(new char[] {' ', '\t', ';', '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

            // Extract the individual values from the dataArray and process them as needed
            // ...

            
            //byte[] linesCountBytes = Encoding.ASCII.GetBytes(linesCount);

            //sp.Write(linesCountBytes, 0, linesCountBytes.Length);
           // Console.WriteLine(xbeeMessage);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
    
    private void SerialPrint(string data, SerialPort sp)
    {
        // Append the new data to the leftover of the previous operation
        _buffer += data;

        int index = _buffer.IndexOf("(");
        int end = _buffer.IndexOf(")");
        int start = 0;

        while (index != -1 && end != -1 && end > index)
        {
            var command = _buffer.Substring(index, end+1);
            Console.WriteLine("Buffer: "+_buffer);
            Console.WriteLine("Answer: "+command);

            var linesCount = _csvService.WriteOnLine(command.Substring(1, command.Length - 2)).ToString();
            _buffer = _buffer.Replace(command, "");
            byte[] linesCountBytes = Encoding.ASCII.GetBytes(linesCount);

            //sp.Write(linesCountBytes, 0, linesCountBytes.Length);
            index = _buffer.IndexOf('\n',0);
            end = _buffer.IndexOf('\r',0);
        }
    }
    /*
     <Komanda ID>, <Çalışma müddəti>, <Telemetriya paketlərinin sayı>, <Batareyanın gərginliyi >,
<Hündürlük>, <Sürət>, <Coğrafi en >, <Coğrafi uzunluq>, <Daşıyıcıdan ayrıldığı vaxt>,
<Videogörüntünün müddəti>
     */
}
