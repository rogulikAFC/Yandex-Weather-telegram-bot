using WeatherAPI.Entities;

namespace WeatherAPI.DAL
{
    public interface IPlaceRepository
    {
        Task<IEnumerable<Place>?> GetPlacesOfUserAsync(Guid userId);

        Task<Place?> GetByIdAsync(Guid placeId);

        void Add(Place place);

        void DeleteAsync(Place place);
    }
}
