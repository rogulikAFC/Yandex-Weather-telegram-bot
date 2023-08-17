using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Models
{
    public class PlaceForCreateDto
    {
        [Range(-90, 90)]
        [Required]
        public float Lat { get; set; }

        [Range(-180, 180)]
        [Required]
        public float Lon { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public Guid UserId { get; set; }
    }
}
