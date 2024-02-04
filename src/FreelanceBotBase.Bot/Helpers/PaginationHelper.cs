﻿using FreelanceBotBase.Domain.Product;
using Telegram.Bot.Types.ReplyMarkups;

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
        public static string FormatProductRecords(IEnumerable<ProductRecord> records)
        {
            string separator = new('-', 90);
            return string.Join('\n' + separator + '\n', records.Select(r => $"Продукт: {r.Product}\nЦена: {r.Cost}"));
        }

        /// <summary>
        /// Create an Inline keyboard.
        /// </summary>
        /// <returns>Inline keyboard.</returns>
        public static InlineKeyboardMarkup CreateInlineKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Предыдущая страница", "prev_page"),
                    InlineKeyboardButton.WithCallbackData("Следующая страница", "next_page"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Выбрать", "select"),
                    InlineKeyboardButton.WithCallbackData("Поиск", "search"),
                }
            });
        }
    }
}