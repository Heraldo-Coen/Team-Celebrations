namespace TeamCelebrations.Data.Responses
{
    /// <summary>
    /// For Employee user
    /// </summary>
    public class UnitResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Acronym { get; set; } = string.Empty;

        public Guid? HigherUnitId { get; set; }
    }
}