using WeatherAPI.Entities;

namespace WeatherAPI.DAL
{
    public interface IPlaceRepository
    {
        Task<IEnumerable<Place>?> GetPlacesOfUserAsync(long userId);

        Task<Place?> GetByIdAsync(Guid placeId);

        void Add(Place place);

        void DeleteAsync(Place place);
    }
}
