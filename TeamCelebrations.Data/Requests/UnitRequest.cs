namespace TeamCelebrations.Data.Requests
{
    public class UnitRequest
    {
        public string? Name { get; set; }

        public string? Acronym { get; set; }

        public Guid? HigherUnitId { get; set; }
    }
}