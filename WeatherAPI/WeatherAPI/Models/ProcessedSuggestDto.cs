namespace WeatherAPI.Models
{
    public class ProcessedSuggestDto
    {
        public ProcessedSuggestDto(string name, int index)
        {
            Name = name;
            Index = index;
        }

        public string Name { get; set; } = null!;
        public int Index { get; set; }
    }
}
