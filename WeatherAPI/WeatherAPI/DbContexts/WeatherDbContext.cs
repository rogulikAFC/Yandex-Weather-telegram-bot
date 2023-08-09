using Microsoft.EntityFrameworkCore;
using WeatherAPI.Entities;

namespace WeatherAPI.DbContexts
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Place> Places { get; set; }
    }
}
