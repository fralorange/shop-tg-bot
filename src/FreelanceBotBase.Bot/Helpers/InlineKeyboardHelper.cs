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
        /// Creates an Inline keyboard that resets whole thing.
        /// </summary>
        /// <returns>Inline keyboard.</returns>
        public static InlineKeyboardMarkup CreateResetInlineKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Вернуться назад", "reset"),
                }
            });
        }

        /// <summary>
        /// Create and Inline keyboard that allows user return menu.
        /// </summary>
        /// <returns></returns>
        public static InlineKeyboardMarkup CreateBackInlineKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Вернуться назад", "back"),
                }
            });
        }

        public static InlineKeyboardMarkup CreateCartInlineKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Удалить", "delete"),
                    InlineKeyboardButton.WithCallbackData("Очистить", "clear"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Оформить заказ", "checkout")
                }
            });
        }
    }
}
