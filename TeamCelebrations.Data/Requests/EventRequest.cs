namespace TeamCelebrations.Data.Requests
{
    public class EventRequest
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; } = DateTime.MinValue;

        #region TEMPORAL

        public Guid? EmployeeId { get; set; }

        public Guid? AdministratorId { get; set; }

        #endregion
    }
}