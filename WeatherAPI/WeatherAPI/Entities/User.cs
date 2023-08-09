using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherAPI.Entities
{
    [Index(nameof(TelegramId), IsUnique = true)]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(32)]
        [Required]
        public string Name { get; set; } = null!;
        
        [Required]
        public int TelegramId { get; set; }

        public virtual ICollection<Place> Places { get; set; } = new List<Place>();
    }
}

