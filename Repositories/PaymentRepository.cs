using week2_Task.Data;
using week2_Task.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDBContext _context;
    private readonly ILogger<PaymentRepository> _logger;

    public PaymentRepository(ApplicationDBContext context, ILogger<PaymentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Payment> GetByIdAsync(Guid id)
    {
        var payment = await _context.Payments.Include(p => p.Merchant).FirstOrDefaultAsync(p => p.PaymentId == id);

        //logging in case of no payment
        if (payment == null)
        {
            _logger.LogWarning("Payment with ID {PaymentId} not found.", id);
        }
       
        return payment;

    }

    public async Task<List<Payment>> GetAllAsync()
    {
        return await _context.Payments.Include(p => p.Merchant).ToListAsync();

    }

    public async Task AddAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
    }

    public async Task UpdateAsync(Payment payment)
    {
        _context.Payments.Update(payment);
    }

    public async Task DeleteAsync(Payment payment)
    {
        _context.Payments.Remove(payment);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}


