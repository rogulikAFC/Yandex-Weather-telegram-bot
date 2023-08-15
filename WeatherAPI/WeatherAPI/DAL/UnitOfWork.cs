using WeatherAPI.DbContexts;

namespace WeatherAPI.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WeatherDbContext _weatherDbContext;

        public UnitOfWork(WeatherDbContext weatherDbContext)
        {
            _weatherDbContext = weatherDbContext
                ?? throw new ArgumentNullException(nameof(weatherDbContext));

            PlaceRepository = new PlaceRepository(_weatherDbContext);
            UserRepository = new UserRepository(_weatherDbContext);
        }

        public IUserRepository UserRepository { get; set; }
        public IPlaceRepository PlaceRepository { get; set; }

        public async Task<bool> SaveChangesAsync()
        {
            return await _weatherDbContext.SaveChangesAsync() >= 0;
        }
    }
}
