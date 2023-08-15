using WeatherAPI.Entities;

namespace WeatherAPI.DAL
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync(
            int pageNum, int pageSize);

        Task<User?> GetByIdAsync(Guid id);

        void Add(User user);

        Task DeleteAsync(Guid id);
    }
}
