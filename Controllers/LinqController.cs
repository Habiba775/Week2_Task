using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using week2_Task.Services;

namespace week2_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinqController : ControllerBase
    {

        private readonly LinqDemoService _service;

        public LinqController(LinqDemoService service)
        {
            _service = service;
        }

        [HttpGet("payments/status/{status}")]
        public async Task<IActionResult> GetByStatus(int status) =>
            Ok(await _service.GetPaymentsByStatusAsync(status));
    }
}
