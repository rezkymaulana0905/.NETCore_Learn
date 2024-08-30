namespace PTC.Models
{
    public class Log
    {
        public DateOnly Date { get; set; }
        public required string Name { get; set; }
        public TimeOnly? Time { get; set; }
        public required string Detail { get; set; }
        public required string Type { get; set; }
    }
}
