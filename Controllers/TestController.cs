using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;


[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminOnly() => Ok("Admin");

    [HttpGet("user")]
    [Authorize(Roles = "User,Admin")]
    public IActionResult UserOrAdmin() => Ok(" User");
}
