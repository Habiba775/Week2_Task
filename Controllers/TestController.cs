using Microsoft.AspNetCore.Mvc;
using Serilog;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        Log.Information(" Logging from controller at {Time}", DateTime.UtcNow);
        return Ok("Log written");
    }
}