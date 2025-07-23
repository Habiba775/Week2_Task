using week2_Task.Repositories;
using week2_Task.Models.Entities;
using week2_Task.Models.DTOS;


namespace week2_Task.Services


{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetAllAsync();
        Task<Transaction> GetByIdAsync(Guid id);
        Task<Transaction> AddAsync(TransactionRequestDTO dto);  
        Task<bool> DeleteAsync(Guid id);
    }
}

