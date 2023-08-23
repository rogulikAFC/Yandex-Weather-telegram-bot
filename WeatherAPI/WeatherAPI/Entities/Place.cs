using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherAPI.Entities
{
    public class Place
    {
        public Place() { }

        public Place(
            Guid id, double lat, double lon,
            string name, long userId)
        {
            Id = id;
            Lat = lat;
            Lon = lon;
            Name = name;
            UserId = userId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Range(-90, 90)]
        public double Lat { get; set; }

        [Required]
        [Range(-180, 180)]
        public double Lon { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public long UserId { get; set; }

        public User User { get; set; } = null!;
    }
}
