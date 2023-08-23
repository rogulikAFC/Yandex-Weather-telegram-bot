using WeatherAPI.Entities;

namespace WeatherAPI.DAL
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync(
            int pageNum, int pageSize);

        /* Task<User?> GetByIdAsync(Guid id); */

        Task<User?> GetUserByTelegramId(long userId);

        void Add(User user);

        void DeleteAsync(User user);
    }
}
