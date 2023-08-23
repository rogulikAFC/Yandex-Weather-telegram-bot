using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherAPI.Entities
{
    [Index(nameof(Id), IsUnique = true)]
    public class User
    {
        public User() { }

        public User(string name, long id)
        {
            Id = id;
        }

        [Key]
        [Required]
        public long Id { get; set; }

        /* [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } */ 

        public virtual ICollection<Place> Places { get; set; }
            = new List<Place>();
    }
}

