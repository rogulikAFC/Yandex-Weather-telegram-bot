using Microsoft.EntityFrameworkCore;
using WeatherAPI.DbContexts;
using WeatherAPI.Entities;

namespace WeatherAPI.DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly WeatherDbContext _weatherDbContext;

        public UserRepository(WeatherDbContext weatherDbContext)
        {
            _weatherDbContext = weatherDbContext
                ?? throw new ArgumentNullException(nameof(weatherDbContext));
        }

        public void Add(User user)
        {
            _weatherDbContext.Users.Add(user);
        }

        public void DeleteAsync(User user)
        {
            _weatherDbContext.Users.Remove(user);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _weatherDbContext.Users
                .Where(u => u.Id == id)
                .Include(u => u.Places)
                .FirstAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync(int pageNum, int pageSize)
        {
            return await _weatherDbContext.Users
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .Include(u => u.Places)
                .ToListAsync();
        }
    }
}
