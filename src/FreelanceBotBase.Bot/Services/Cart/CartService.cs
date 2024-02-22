using AutoMapper;
using FreelanceBotBase.Contracts.Product;
using FreelanceBotBase.Domain.Product;
using System.Collections.Concurrent;

namespace FreelanceBotBase.Bot.Services.Cart
{
    /// <inheritdoc cref="ICartService"/>
    public class CartService : ICartService
    {
        private readonly ConcurrentDictionary<long, List<ProductRecord>> _carts;
        private readonly IMapper _mapper;

        /// <inheritdoc cref="ICartService"/>
        public CartService(IMapper mapper)
        {
            _carts = new ConcurrentDictionary<long, List<ProductRecord>>();
            _mapper = mapper;
        }

        public IEnumerable<ProductDto>? Get(long userId)
        {
            if (_carts.TryGetValue(userId, out var cart))
            {
                return _mapper.Map<List<ProductDto>>(cart);
            }
            return null;
        }

        public ProductDto? Get(long userId, string productName)
        {
            if (_carts.TryGetValue(userId, out var cart))
            {
                var product = cart.FirstOrDefault(p => p.Product.Equals(productName));
                if (product != null)
                {
                    return _mapper.Map<ProductDto>(product);
                }
            }
            return null;
        }

        public void Add(long userId, ProductDto productRecord)
        {
            var cart = _carts.GetOrAdd(userId, new List<ProductRecord>());
            var product = _mapper.Map<ProductRecord>(productRecord);
            cart.Add(product);
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
