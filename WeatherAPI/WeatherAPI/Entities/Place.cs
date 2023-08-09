using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherAPI.Entities
{
    public class Place
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public float Lat { get; set; }

        [Required]
        public float Lon { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public Guid UserId { get; set; }
        
        public User User { get; set; } = null!;
    }
}
