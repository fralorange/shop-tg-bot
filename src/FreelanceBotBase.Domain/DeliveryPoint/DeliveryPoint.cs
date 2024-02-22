namespace FreelanceBotBase.Domain.DeliveryPoint
{
    /// <summary>
    /// Delivery Point.
    /// </summary>
    public class DeliveryPoint
    {
        /// <summary>
        /// Delivery point id.
        /// </summary>
        public required long Id { get; set; }
        /// <summary>
        /// Delivery point name.
        /// </summary>
        public required string Name { get; set; } 
        /// <summary>
        /// Delivery point location.
        /// </summary>
        public required string Location { get; set; }
        /// <summary>
        /// Delivery point Manager id.
        /// </summary>
        public long? ManagerId { get; set; }
        /// <summary>
        /// Navigation variable for User.
        /// </summary>
        public virtual User.User? Manager { get; set; }
    }
}
