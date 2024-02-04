namespace FreelanceBotBase.Bot.Helpers
{
    /// <summary>
    /// Helper for paginating data.
    /// </summary>
    public static class PaginationHelper<TModel>
    {
        /// <summary>
        /// Split collection by pages.
        /// </summary>
        /// <param name="records">Collection.</param>
        /// <param name="pageSize">Page size (How many records on one page).</param>
        /// <param name="pageIndex">Page index.</param>
        /// <returns></returns>
        public static IEnumerable<TModel> SplitByPages(IEnumerable<TModel> records, int pageSize, int pageIndex)
            => records.Skip((pageIndex - 1) * pageSize).Take(pageSize);
    }
}
