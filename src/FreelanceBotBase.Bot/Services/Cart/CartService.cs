using FreelanceBotBase.Domain.Product;
using System.Collections.Concurrent;

namespace FreelanceBotBase.Bot.Services.Cart
{
    public class CartService : ICartService
    {
        private ConcurrentDictionary<long, List<ProductRecord>> _carts;

        public CartService()
            => _carts = new ConcurrentDictionary<long, List<ProductRecord>>();

        public IEnumerable<ProductRecord>? Get(long userId)
        {
            if (_carts.TryGetValue(userId, out var cart))
                return cart;
            return null;
        }

        public ProductRecord? Get(long userId, string productName)
        {
            if (_carts.TryGetValue(userId, out var cart))
                return cart.FirstOrDefault(p => p.Product.Equals(productName));
            return null;
        }

        public void Add(long userId, ProductRecord productRecord)
        {
            var cart = _carts.GetOrAdd(userId, new List<ProductRecord>());
            cart.Add(productRecord);
        }

        public bool Delete(long userId)
        {
            return _carts.TryRemove(userId, out _);
        }

        public bool Delete(long userId, string productName)
        {
            if (_carts.TryGetValue(userId, out var cart))
            {
                var product = cart.FirstOrDefault(p => p.Product.Equals(productName));
                if (product != null)
                {
                    cart.Remove(product);
                    return true;
                }
            }
            return false;
        }
    }
}
