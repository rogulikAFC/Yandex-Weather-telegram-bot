namespace WeatherAPI.DAL
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; set; }
        IPlaceRepository PlaceRepository { get; set; }
        Task<bool> SaveChangesAsync();
    }
}
