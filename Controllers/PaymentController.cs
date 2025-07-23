using Microsoft.AspNetCore.Mvc;
using week2_Task.Models.DTOS;
using week2_Task.Models.Entities.Enums;
using week2_Task.Services;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPayments()
    {
        var payments = await _paymentService.GetAllPaymentsAsync();
        return Ok(payments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaymentById(Guid id)
    {
        var payment = await _paymentService.GetPaymentDetails(id);
        if (payment == null)
            return NotFound($"No payment found with ID {id}");
        return Ok(payment);
    }

    [HttpPost]
    public async Task<IActionResult> AddPayment([FromBody] AddPaymentDTO dto)
    {
        var payment = await _paymentService.AddPaymentAsync(dto);
        if (payment == null)
            return BadRequest("Invalid MerchantId");

        return Ok(new PaymentResponseDTO
        {
            Id = payment.PaymentId,
            Amount = payment.Amount,
            Currency = payment.Currency,
            Method = payment.Method,
            Status = (Status)payment.Status,
            MerchantId = payment.MerchantId
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayment(Guid id)
    {
        var success = await _paymentService.DeletePaymentAsync(id);
        if (!success)
            return NotFound();

        return Ok($"Payment with ID {id} deleted");
    }
}
