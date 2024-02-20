namespace FreelanceBotBase.Domain.DeliveryPoint
{
    /// <summary>
    /// Delivery Point.
    /// </summary>
    public class DeliveryPoint
    {
        public required long Id { get; set; }
        public required string Name { get; set; } 
        public required string Location { get; set; }
    }
}
