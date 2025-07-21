using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using week2_Task.Data;
using week2_Task.Models.Entities;
using week2_Task.Models.Entities.Enums;


namespace week2_Task.Services
{
    public class LinqDemoService
    {
        private readonly ApplicationDBContext _dbContext;

        public LinqDemoService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Payment>> GetPaymentsByStatusAsync(int status)
        {
            return await _dbContext.Payments
                .Include(p => p.Merchant)
                .Where(payment => payment.Status == (Status)status)
                .ToListAsync();
        }

        // query syntax
        /* var query = from payment in _dbContext.Payments
            where payment.Status == (Status)status
            select payment;

return await query.ToListAsync();*/

    }
}
