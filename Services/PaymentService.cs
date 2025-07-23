using week2_Task.Models.DTOS;
using week2_Task.Models.Entities;
using week2_Task.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repo;

    public PaymentService(IPaymentRepository repo)
    {
        _repo = repo;
    }

    public async Task<Payment> GetPaymentDetails(Guid id)
    {
        return await _repo.GetByIdAsync(id);
    }

    public async Task<List<Payment>> GetAllPaymentsAsync()
    {
        return await _repo.GetAllAsync();
    }

    public async Task<Payment> AddPaymentAsync(AddPaymentDTO dto)
    {
        var payment = new Payment
        {
            PaymentId = Guid.NewGuid(),
            Amount = dto.Amount,
            Currency = dto.Currency,
            Method = dto.Method,
            Status = dto.Status,
            MerchantId = dto.MerchantId
        };

        await _repo.AddAsync(payment);
        await _repo.SaveChangesAsync();
        return payment;
    }

    public async Task<bool> DeletePaymentAsync(Guid id)
    {
        var payment = await _repo.GetByIdAsync(id);
        if (payment == null)
            return false;

        await _repo.DeleteAsync(payment);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<Payment> UpdatePaymentAsync(Guid id, UpdatePaymentDTO dto)
    {
        var payment = await _repo.GetByIdAsync(id);
        if (payment == null)
            return null;

        payment.Amount = dto.Amount;
        payment.Currency = dto.Currency;
        payment.Method = dto.Method;
        payment.Status = dto.Status;
        payment.ProcessedDate = dto.ProcessedDate;

        await _repo.UpdateAsync(payment);
        await _repo.SaveChangesAsync();
        return payment;
    }
}



