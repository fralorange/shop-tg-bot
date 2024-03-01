namespace FreelanceBotBase.Contracts.User
{
    public class UserDto
    {
        /// <summary>
        /// User id.
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// User enumerated role.
        /// </summary>
        public int UserRole { get; set; }
        /// <summary>
        /// Nullable delivery point id.
        /// </summary>
        public long? DeliveryPointId { get; set; }
    }
}
