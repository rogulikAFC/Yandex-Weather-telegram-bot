using WeatherAPI.Entities;

namespace WeatherAPI.DAL
{
    public interface IPlaceRepository
    {
        Task<IEnumerable<Place>?> GetPlacesOfUserAsync(
            Guid userId, int pageNum, int pageSize);

        Task<Place?> GetByIdAsync(Guid placeId);

        void Add(Place place);

        Task DeleteAsync(Guid placeId);
    }
}
