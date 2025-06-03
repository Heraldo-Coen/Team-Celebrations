using TeamCelebrations.Data.Entities;

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

        public UnitResponse()
        {
        }

        public UnitResponse(Unit unit)
        {
            Id = unit.Id;
            Name = unit.Name;
            Acronym = unit.Acronym;
            HigherUnitId = unit.HigherUnitId;
        }
    }
}