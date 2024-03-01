using FreelanceBotBase.Contracts.DeliveryPoint;
using FreelanceBotBase.Contracts.Product;
using FreelanceBotBase.Contracts.User;
using FreelanceBotBase.Domain.DeliveryPoint;

namespace FreelanceBotBase.Bot.Helpers
{
    /// <summary>
    /// Helper for paginating data.
    /// </summary>
    public static class PaginationHelper
    {
        /// <summary>
        /// Split collection by pages.
        /// </summary>
        /// <param name="records">Collection.</param>
        /// <param name="pageSize">Page size (How many records on one page).</param>
        /// <param name="pageIndex">Page index.</param>
        /// <returns></returns>
        public static IEnumerable<TModel> SplitByPages<TModel>(IEnumerable<TModel> records, int pageSize, int pageIndex)
            => records.Skip((pageIndex - 1) * pageSize).Take(pageSize);

        /// <summary>
        /// Format data by hard-coded pattern.
        /// </summary>
        /// <param name="records">Records that are being formatted.</param>
        /// <returns>Formatted data.</returns>
        public static string Format(IEnumerable<ProductDto> records)
        {
            string separator = new('-', 90);
            return string.Join('\n' + separator + '\n', records.Select(r => $"Продукт: {r.Product}\nЦена: {r.Cost}"));
        }

        public static string Format(IEnumerable<UserDto> users)
        {
            string separator = new('-', 90);
            return string.Join('\n' + separator + '\n', users.Select(u => $"Id: {u.UserId}\nРоль: {(Domain.User.User.Role)u.UserRole}"));
        }

        public static string Format(IEnumerable<DeliveryPointDto> points)
        {
            var separator = new string('-', 90);
            return string
                .Join('\n' + separator + '\n', points.Select(dp => $"Номер: {dp.Id}\nНазвание пункта: {dp.Name}\nЛокация: {dp.Location}\nПустой: {dp.ManagerId == null}"));
        }
    }
}
