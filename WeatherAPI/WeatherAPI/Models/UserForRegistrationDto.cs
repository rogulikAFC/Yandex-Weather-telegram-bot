using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Models
{
    public class UserForRegistrationDto
    {
        [Required]
        public long Id { get; set; }
    }
}
