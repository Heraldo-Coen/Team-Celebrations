namespace TeamCelebrations.Data.Requests
{
    public class FriendshipRequest
    {
        /// <summary>
        /// For the employee who requested the friendship.
        /// </summary>
        public Guid? EmployeeId1 { get; set; }

        /// <summary>
        /// For the employee who received the friendship request.
        /// </summary>
        public Guid? EmployeeId2 { get; set; }
    }
}