namespace FreelanceBotBase.Domain.Product
{
    /// <summary>
    /// Product record from google sheet.
    /// </summary>
    public class ProductRecord
    {
        public required string Product { get; set; }
        public required int Cost { get; set; }
    }
}
