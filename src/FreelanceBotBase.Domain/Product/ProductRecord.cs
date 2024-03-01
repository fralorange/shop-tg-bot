namespace FreelanceBotBase.Domain.Product
{
    /// <summary>
    /// Product record from google sheet.
    /// </summary>
    public class ProductRecord
    {
        /// <summary>
        /// Product name.
        /// </summary>
        public required string Product { get; set; }
        /// <summary>
        /// Product cost.
        /// </summary>
        public required int Cost { get; set; }
    }
}
