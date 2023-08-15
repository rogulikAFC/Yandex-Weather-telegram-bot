using Microsoft.EntityFrameworkCore;
using WeatherAPI.DbContexts;
using WeatherAPI.Entities;

namespace WeatherAPI.DAL
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly WeatherDbContext _weatherDbContext;

        public PlaceRepository(WeatherDbContext weatherDbContext)
        {
            _weatherDbContext = weatherDbContext
                ?? throw new ArgumentNullException(nameof(weatherDbContext));
        }

        public void Add(Place place)
        {
            _weatherDbContext.Places.Add(place);
        }

        public async Task DeleteAsync(Guid placeId)
        {
            var place = await _weatherDbContext
                .Places.FindAsync(placeId);

            if (place == null)
            {
                return;
            }

            _weatherDbContext.Places.Remove(place);
        }

        public async Task<Place?> GetByIdAsync(Guid placeId)
        {
            return await _weatherDbContext
                .Places.FindAsync(placeId);
        }

        public async Task<IEnumerable<Place>?> GetPlacesOfUserAsync(Guid userId, int pageNum, int pageSize)
        {
            var user = await _weatherDbContext.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Places)
                .FirstAsync();

            if (user == null)
            {
                return null;
            }

            return user.Places
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}
