namespace TeamCelebrations.Data.Entities
{
    public class Event : BaseEntity
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; } = DateTime.MinValue;
    }
}