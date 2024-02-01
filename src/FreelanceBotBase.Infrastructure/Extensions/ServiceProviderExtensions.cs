using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FreelanceBotBase.Infrastructure.Extensions
{
    /// <summary>
    /// Extension class for ServiceProvider.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Gets configuration from service provider.
        /// </summary>
        /// <typeparam name="T">Configuration type.</typeparam>
        /// <param name="serviceProvider">ServiceProvider.</param>
        /// <returns>Configuration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T GetConfiguration<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            var o = serviceProvider.GetService<IOptions<T>>() ?? throw new ArgumentNullException(nameof(T));
            return o.Value;
        }
    }
}