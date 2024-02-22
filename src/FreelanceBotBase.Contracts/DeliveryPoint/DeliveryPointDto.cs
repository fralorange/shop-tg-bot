namespace FreelanceBotBase.Contracts.DeliveryPoint
{
    /// <summary>
    /// Delivery Point DTO.
    /// </summary>
    public class DeliveryPointDto
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
    }
}
