using FreelanceBotBase.Contracts.Product;
using Microsoft.Extensions.Caching.Memory;

namespace FreelanceBotBase.Infrastructure.Helpers
{
    public class CacheHelper
    {
        private readonly IMemoryCache _cache;

        public CacheHelper(IMemoryCache cache)
            => _cache = cache;

        public IEnumerable<ProductDto>? GetRecords(long chatId, out int currentPage, string userInput = null)
        {
            var records = _cache.Get<IEnumerable<ProductDto>>($"{chatId}_records");
            currentPage = 1;

            if (records == null)
                return null;

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
                SlidingExpiration = TimeSpan.FromMinutes(15),
            };

            if (!string.IsNullOrEmpty(userInput))
                records = records.Where(r => r.Product.StartsWith(userInput)).ToList();

            _cache.Set($"{chatId}_records", records, cacheOptions.SetSize(10));

            _cache.Set($"{chatId}_currentPage", currentPage, cacheOptions);

            return records;
        }
    }
}
