namespace TeamCelebrations.Data.Responses
{
    /// <summary>
    /// For Employee user
    /// </summary>
    public class FriendshipResponse
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid FriendId { get; set; }

        public bool IsConfirmed { get; set; }

        /// <summary>
        /// Use CreatedAt for the date the friendship was created
        /// </summary>
        public DateTime SendAt { get; set; }
    }
}