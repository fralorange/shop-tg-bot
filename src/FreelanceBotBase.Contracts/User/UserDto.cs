namespace FreelanceBotBase.Contracts.User
{
    public class UserDto
    {
        public long UserId { get; set; }
        public int UserRole { get; set; }
        public long? DeliveryPointId { get; set; }
    }
}
