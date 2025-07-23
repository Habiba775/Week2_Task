using week2_Task.Models.DTOS;
using week2_Task.Models.Entities;


namespace week2_Task.Services
{
    public interface IPaymentService
    {
        Task<Payment> GetPaymentDetails(Guid id);
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<Payment> AddPaymentAsync(AddPaymentDTO dto);
        Task<bool> DeletePaymentAsync(Guid id);
        Task<Payment> UpdatePaymentAsync(Guid id, UpdatePaymentDTO dto);

    }
}
