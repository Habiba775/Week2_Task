using week2_Task.Data;
using week2_Task.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace week2_Task.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDBContext _db;

        public AuthRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task<User?> GetUserAsync(string email, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;

            return user;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

    }
}

