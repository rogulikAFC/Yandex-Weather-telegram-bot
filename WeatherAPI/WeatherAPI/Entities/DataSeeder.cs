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
            if (!_weatherDbContext.Places.Any())
            {
                var places = new List<Place>()
                {
                    new Place(
                        Guid.NewGuid(), 1, 1,"Somewhere in the Africa",
                        new Guid("507e820e-1081-49f1-aa85-c1227e57a79e")),

                    new Place(
                        Guid.NewGuid(), 10.5153, 78.314, "A hill near the Madurai",
                        new Guid("cee2057f-0c13-4802-b7fd-a4e0e739cbd3"))
                };

                _weatherDbContext.Places.AddRange(places);
                _weatherDbContext.SaveChanges();
            }

            if (!_weatherDbContext.Users.Any())
            {
                var users = new List<User>()
                {
                    new User(Guid.NewGuid(), "rogulik", 5007789926),
                    new User(Guid.NewGuid(), "second user", 1111111111)
                };

                _weatherDbContext.Users.AddRange(users);
                _weatherDbContext.SaveChanges();
            }
        }
    }
}
