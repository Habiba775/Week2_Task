using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using week2_Task.Data;
using week2_Task.Models.DTOS;
using week2_Task.Models.Entities;
using System.ComponentModel.DataAnnotations;


namespace week2_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;

        public PaymentController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllPayments()
        {
            try
            {
                var payments = _dbContext.Payments
                    .Include(p => p.Merchant)
                    .ToList();

                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching payments: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public IActionResult GetPaymentById(Guid id)
        {
            var payment = _dbContext.Payments
                .Include(p => p.Merchant)
                .FirstOrDefault(p => p.Id == id);

            if (payment == null)
            {
                return NotFound($"No payment found with ID {id}");
            }

            return Ok(payment);
        }


        [HttpPost]
        public IActionResult AddPayment([FromBody] AddPaymentDTO dto)
        {
            var merchant = _dbContext.Merchants.Find(dto.MerchantId);
            if (merchant == null)
                return BadRequest("Invalid MerchantId");

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                Amount = dto.Amount,
                Currency = dto.Currency,
                Method = dto.Method,
                Status = dto.Status,
                Merchant = merchant
            };

            _dbContext.Payments.Add(payment);
            _dbContext.SaveChanges();

            return Ok(new PaymentResponseDTO
            {
                Id = payment.Id,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Method = payment.Method,
                Status = payment.Status,
                MerchantId = payment.Merchant.Id
            });
        }

        // PUT: api/payment/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePayment(Guid id, [FromBody] UpdatePaymentDTO updateDto)
        {
            var payment = _dbContext.Payments
       .Include(p => p.Merchant) 
       .FirstOrDefault(p => p.Id == id);

            if (payment == null)
                return NotFound();

            payment.Amount = updateDto.Amount;
            payment.Currency = updateDto.Currency;
            payment.Method = updateDto.Method;
            payment.Status = updateDto.Status;
            payment.ProcessedDate = updateDto.ProcessedDate;
            




            _dbContext.SaveChanges();

            return Ok(payment);
        }

        // DELETE: api/payment/{id}
        [HttpDelete("{id:int}")]
        public IActionResult DeletePayment(int id)
        {
            var payment = _dbContext.Payments.Find(id);

            if (payment == null)
                return NotFound();

            _dbContext.Payments.Remove(payment);
            _dbContext.SaveChanges();

            return Ok(payment);
        }
    }
}

