using week2_Task.Models.Entities;

namespace week2_Task.Repositories
{


    public interface IAuthRepository
    {
        Task<User?> GetUserAsync(string email, string password);
        Task<User> CreateUserAsync(User user);
       

    }
}