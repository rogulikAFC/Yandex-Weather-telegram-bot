using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Models
{
    public class PlaceForUpdateDto
    {
        /*[Range(-90, 90)]
        [Required]
        public float Lat { get; set; }

        [Range(-180, 180)]
        [Required]
        public float Lon { get; set; } */

        public bool IsMain { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = null!;
    }
}
