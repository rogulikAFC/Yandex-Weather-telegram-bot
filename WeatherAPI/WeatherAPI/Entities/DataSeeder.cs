using WeatherAPI.DbContexts;

namespace WeatherAPI.Entities
{
    public class DataSeeder
    {
        private readonly WeatherDbContext _weatherDbContext;

        public DataSeeder(WeatherDbContext weatherDbContext)
        {
            _weatherDbContext = weatherDbContext
                ?? throw new ArgumentNullException(nameof(weatherDbContext));
        }

        public void Seed()
        {

            if (!_weatherDbContext.Users.Any())
            {
                var users = new List<User>()
                {
                    new User( /* Guid.NewGuid() ,*/ "rogulik", 5007789926),
                    new User( /* Guid.NewGuid() ,*/ "second user", 1111111111)
                };

                _weatherDbContext.Users.AddRange(users);
                _weatherDbContext.SaveChanges();
            }

            if (!_weatherDbContext.Places.Any())
            {
                var places = new List<Place>()
                {
                    new Place(
                        Guid.NewGuid(), 1, 1,"Somewhere in the Africa",
                        1111111111),

                    new Place(
                        Guid.NewGuid(), 10.5153, 78.314, "A hill near the Madurai",
                        5007789926)
                };

                _weatherDbContext.Places.AddRange(places);
                _weatherDbContext.SaveChanges();
            }
        }
    }
}
