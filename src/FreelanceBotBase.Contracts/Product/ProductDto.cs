namespace FreelanceBotBase.Contracts.Product
{
    public class ProductDto
    {
        /// <summary>
        /// Product name.
        /// </summary>
        public string Product { get; set; } = string.Empty;
        /// <summary>
        /// Product cost.
        /// </summary>
        public int Cost { get; set; }
    }
}
