using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using week2_Task.Data;
using week2_Task.Models.DTOS;
using week2_Task.Models.Entities;

namespace week2_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;

        public MerchantController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        //asynch method used when with external methods and non blocking operations
        public async Task<IActionResult> GetAllAsync()
        {
            var merchants = await _dbContext.Merchants.ToListAsync();
            return Ok(merchants);
        }
        //sync block the thread while waiting for the db
        //so its fast and simple as it gets by id one row
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetAllMerchantsById(Guid id)
        {
            var merchant = _dbContext.Merchants.Find(id);
            if (merchant == null)
            {
                return NotFound();
            }
            return Ok(merchant);
        }

        //async as it takes some time so non blocking 

        [HttpPost]
        public async Task<IActionResult> AddMerchantAsync([FromBody] AddMerchantDTO addMerchantDto)
        {
            if (!ModelState.IsValid || addMerchantDto == null)
                return BadRequest(ModelState);

            var merchantEntity = new Merchant
            (
                addMerchantDto.Name,
                addMerchantDto.Email,
                addMerchantDto.Phone,
                addMerchantDto.Address
            );

          
            await _dbContext.Merchants.AddAsync(merchantEntity);

            
            await _dbContext.SaveChangesAsync();

            return Ok(merchantEntity);
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


