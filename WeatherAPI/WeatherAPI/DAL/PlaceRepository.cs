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

        public void DeleteAsync(Place place)
        {
            _weatherDbContext.Places.Remove(place);
        }

        public async Task<Place?> GetByIdAsync(Guid placeId)
        {
            return await _weatherDbContext.Places
                .Where(p => p.Id == placeId)
                .Include(p => p.User)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Place>?> GetPlacesOfUserAsync(Guid userId)
        {
            var user = await _weatherDbContext.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Places)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            return user.Places;
        }
    }
}
