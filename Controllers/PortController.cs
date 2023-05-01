using CansatGround.Models;
using CansatGround.Services;
using Microsoft.AspNetCore.Mvc;

namespace CansatGround.Controllers;

[ApiController]
[Route("port")]
public class PortController : Controller
{
    private CPropsService _cpropsService { get; set; }
    private XbeeService _xbeeService { get; set; }
    public PortController(CPropsService cpropsService, XbeeService xbeeService)
    {
        _cpropsService = cpropsService;
        _xbeeService = xbeeService;
    }
        
    [Route("getlist")]
    [HttpGet]
    public string[] GetList()
    {
        return _cpropsService.GetCOMPorts();
    }
    
    [Route("connect")]
    [HttpGet]
    public IActionResult Connect(string port)
    {
        try
        {
            _cpropsService.SetCOMPort(port);
            _xbeeService.RunSerialPort(port);
            return Ok();
        }
        catch (InvalidOperationException)
        {
            return StatusCode(99);
        }
        catch (Exception e)
        {
            return StatusCode(e.HResult, e);
        }
    }
    
    [Route("disconnect")]
    [HttpGet]
    public IActionResult Disconnect(string port)
    {
        try
        {
            _xbeeService.ClosePort(port);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(e.HResult, e);
        }
    }
}