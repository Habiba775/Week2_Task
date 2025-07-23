using week2_Task.Models.Entities;

public interface IPaymentRepository
{
    Task<Payment> GetByIdAsync(Guid id);
    Task<List<Payment>> GetAllAsync();
    Task AddAsync(Payment payment);
    Task UpdateAsync(Payment payment);
    Task DeleteAsync(Payment payment);
    Task SaveChangesAsync();
}

