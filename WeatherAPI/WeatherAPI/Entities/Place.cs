using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherAPI.Entities
{
    public class Place
    {
        public Place(
            Guid id, double lat, double lon,
            string name, Guid userId)
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
        public double Lat { get; set; }

        [Required]
        public double Lon { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public Guid UserId { get; set; }
        
        public User User { get; set; } = null!;
    }
}
