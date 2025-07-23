
using week2_Task.Models.Entities;
using week2_Task.Models.DTOS;
using week2_Task.Repositories;


namespace week2_Task.Services;
public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;

    public TransactionService(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Transaction> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Transaction> AddAsync(TransactionRequestDTO dto)
    {
        var transaction = new Transaction(dto.PaymentId, dto.TransactionReference, dto.ProcessedAmount, dto.Description, dto.Notes);
        return await _repository.AddAsync(transaction);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}

