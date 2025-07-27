using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using week2_Task.Data;
using week2_Task.Models.DTOS;
using week2_Task.Models.Entities;

namespace week2_Task.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MerchantController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;

        public MerchantController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            var merchants = await _dbContext.Merchants.ToListAsync();
            return Ok(merchants);
        }
        
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetMerchantById(Guid id)
        {
            var merchant = _dbContext.Merchants.FirstOrDefault(m => m.Id == id);

            if (merchant == null)
            {
                return NotFound($"Merchant with ID {id} was not found.");
            }

            return Ok(merchant);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddMerchant([FromBody] AddMerchantDTO dto)
        {
            var merchant = new Merchant
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                CreatedDate = DateTime.UtcNow
            };

            try
            {
                _dbContext.Merchants.Add(merchant);
                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    message = "Merchant created successfully",
                    merchantId = merchant.Id
                });
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Merchants_Email_Unique") == true)
            {
                return Conflict("Email already exists. Please use a different one.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }


        //sync simple
        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateMerchant(Guid id, [FromBody] UpdateMerchantDTO updateMerchantDto)
        {
            var merchant = _dbContext.Merchants.Find(id);
            if (merchant == null)
            {
                return NotFound();
            }

            merchant.Name = updateMerchantDto.Name;
            merchant.Email = updateMerchantDto.Email;
            merchant.Phone = updateMerchantDto.Phone;
            merchant.Address = updateMerchantDto.Address;

            _dbContext.SaveChanges();

            return Ok(merchant);
        }
        //sync simple
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMerchantAsync(Guid id)
        {
            var merchant = await _dbContext.Merchants.FindAsync(id);
            if (merchant == null)
                return NotFound();

            _dbContext.Merchants.Remove(merchant);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}


