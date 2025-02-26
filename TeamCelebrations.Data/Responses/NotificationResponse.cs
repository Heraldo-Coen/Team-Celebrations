namespace TeamCelebrations.Data.Responses
{
    /// <summary>
    /// For Employee user
    /// </summary>
    public class NotificationResponse
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid? EventId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public DateTime CreateAt { get; set; }
    }
}