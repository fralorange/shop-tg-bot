using FreelanceBotBase.Contracts.Product;
using FreelanceBotBase.Domain.Product;

namespace FreelanceBotBase.Bot.Services.Cart
{
    /// <summary>
    /// Service for managing user carts.
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Gets entire cart for specific user id.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns><see cref="IEnumerable{ProductRecord}"/></returns>
        IEnumerable<ProductDto>? Get(long userId);
        /// <summary>
        /// Gets specific item from user's cart.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="productName">Product name.</param>
        /// <returns><see cref="ProductRecord"/></returns>
        ProductDto? Get(long userId, string productName);
        /// <summary>
        /// Adds to dictionary another user's cart.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="productRecord">Product.</param>
        void Add(long userId, ProductDto productRecord);
        /// <summary>
        /// Deletes entire user's cart.
        /// </summary>
        /// <param name="userId">User id.</param>
        bool Delete(long userId);
        /// <summary>
        /// Deletes specific item from user's cart.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="productName">Product name.</param>
        bool Delete(long userId, string productName);
    }
}
