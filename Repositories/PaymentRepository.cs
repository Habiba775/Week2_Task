using week2_Task.Data;
using week2_Task.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDBContext _context;

    public PaymentRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Payment> GetByIdAsync(Guid id)
    {
        return await _context.Payments.Include(p => p.Merchant).FirstOrDefaultAsync(p => p.PaymentId == id);
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


