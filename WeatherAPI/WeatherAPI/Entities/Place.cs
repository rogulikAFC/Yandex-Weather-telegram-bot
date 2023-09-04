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
        [MaxLength(64)]
        public string Name { get; set; } = null!;

        public byte[] MapImage { get; set; }

        public long UserId { get; set; }

        public User User { get; set; } = null!;

        public bool IsMain { get; set; }

        public void SetMapImage()
        {
            var yandexApiKey = Environment.GetEnvironmentVariable("YANDEX_API_KEY");

            var url = $"https://static-maps.yandex.ru/1.x/" +
                $"?ll={Lon},{Lat}" +
                $"&l=map" +
                $"&z=16" +
                $"&apikey={yandexApiKey}";

            Console.WriteLine(url);

            var client = new HttpClient();

            var response = client.GetAsync(url).Result;

            var image = response.Content.ReadAsByteArrayAsync().Result;

            MapImage = image;
        }
    }
}
