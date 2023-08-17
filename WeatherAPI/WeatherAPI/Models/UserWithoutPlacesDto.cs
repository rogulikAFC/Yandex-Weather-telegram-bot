using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Models
{
    public class UserWithoutPlacesDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(32)]
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public long TelegramId { get; set; }
    }
}
