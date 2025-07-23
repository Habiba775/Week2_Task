using week2_Task.Models.Entities;

namespace week2_Task.Repositories
{
    public interface ITransactionRepository
    {
       
            Task<List<Transaction>> GetAllAsync();
            Task<Transaction> GetByIdAsync(Guid id);
            Task<Transaction> AddAsync(Transaction transaction);
            Task<bool> DeleteAsync(Guid id);
        }
    }

