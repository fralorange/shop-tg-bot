using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Helpers
{
    /// <summary>
    /// Helper for creating various keyboards.
    /// </summary>
    public static class InlineKeyboardHelper
    {
        /// <summary>
        /// Creates an Inline keyboard.
        /// </summary>
        /// <returns>Inline keyboard.</returns>
        public static InlineKeyboardMarkup CreateDefaultInlineKeyboard()
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

        /// <summary>
        /// Creates an Inline keyboard after search procedure.
        /// </summary>
        /// <returns>Inline keyboard.</returns>
        public static InlineKeyboardMarkup CreateSearchInlineKeyboard()
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
                    InlineKeyboardButton.WithCallbackData("Сбросить поиск", "reset"),
                }
            });
        }

        /// <summary>
        /// Creates an Inline keyboard after select procedure.
        /// </summary>
        /// <returns>Inline keyboard.</returns>
        public static InlineKeyboardMarkup CreateSelectInlineKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Подтвердить", "confirm"),
                    InlineKeyboardButton.WithCallbackData("Отменить", "reset"),
                }
            });
        }

        /// <summary>
        /// Creates an Inline keyboard after select failure procedure.
        /// </summary>
        /// <returns>Inline keyboard.</returns>
        public static InlineKeyboardMarkup CreateSelectNotFoundInlineKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Вернуться назад", "reset"),
                }
            });
        }
    }
}
