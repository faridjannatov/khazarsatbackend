using CansatGround.Models;
using System.IO.Ports;
using Newtonsoft.Json;

namespace CansatGround.Services;

public class CPropsService
{
    private IConfiguration _configuration { get; set; }
    private readonly string _propsRoute = $"{AppContext.BaseDirectory}/cansatprops.json";
    public CPropsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public CansatGroundProperties GetProps()
    {
        var cansatGroundProperties = _configuration.To<CansatGroundProperties>("CansatGroundProperties");
        return cansatGroundProperties;
    }

    public void SetProps(CansatGroundProperties cansatGroundProperties)
    {
        using (var sw = new StreamWriter(_propsRoute, true))
        {
            var newText = JsonConvert.SerializeObject(cansatGroundProperties);
            sw.Write(newText);
        }
    }

    public void SetCOMPort(string port)
    {
        var cansatProps = GetProps();
        cansatProps.XbeeProperties.PortName = port;

        SetProps(cansatProps);
    }

    public string[] GetCOMPorts()
    {
        var availablePorts = SerialPort.GetPortNames();
        return availablePorts;
    }
}