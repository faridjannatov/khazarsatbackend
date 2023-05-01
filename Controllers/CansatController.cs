using System.Text.Json;
using CansatGround.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace CansatGround.Controllers;

[ApiController]
[Route("telemetry")]
public class CansatController : Controller
{
    private CSVService _csvService { get; set; }
    public CansatController(CSVService csvService)
    {
        _csvService = csvService;
    }
    
    [HttpGet]
    public JsonResult Get(string op)
    {
        if (op == "read")
        {
            var data = _csvService.CSVRead();
            return Json(data);
        }

        else if (op == "write") _csvService.testcsvwrite();
        return new JsonResult(new object());
    }
}