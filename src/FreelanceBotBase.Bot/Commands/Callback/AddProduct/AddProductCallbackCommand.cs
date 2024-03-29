﻿using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Bot.Services.Cart;
using FreelanceBotBase.Contracts.Product;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Callback.AddProduct
{
    /// <summary>
    /// Add product to cart callback command.
    /// </summary>
    public class AddProductCallbackCommand : CallbackCommandBase
    {
        /// <summary>
        /// Cart service to manage products in user's cart.
        /// </summary>
        private readonly ICartService _cartService;
        /// <summary>
        /// Memory cache.
        /// </summary>
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Create callback command that adds products to cart.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="cartSerivce"></param>
        /// <param name="cache"></param>
        public AddProductCallbackCommand(ITelegramBotClient botClient, ICartService cartSerivce, IMemoryCache cache) : base(botClient)
        {
            _cartService = cartSerivce;
            _cache = cache;
        }

        /// <inheritdoc/>
        public async override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var cart = _cartService.Get(callbackQuery.From.Id);

            if (cart is not null && cart!.Count() == 10)
            {
                return await BotClient.EditMessageTextAsync(
                    chatId: callbackQuery.Message!.Chat.Id,
                    messageId: callbackQuery.Message.MessageId,
                    text: "Маскимальное количество товаров в корзине: 10",
                    replyMarkup: KeyboardHelper.CreateResetInlineKeyboard(),
                    cancellationToken: cancellationToken);
            }

            var records = _cache.Get<IEnumerable<ProductDto>>($"{callbackQuery.Message!.Chat.Id}_records");

            if (records is null)
            {
                return await BotClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: "Время ответа истекло. Пожалуйста, сгенерируйте новый список!",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            }

            string productName = string.Empty;

            string pattern = "\"([^\"]*)\"";
            Match match = Regex.Match(callbackQuery.Message!.Text!, pattern);
            if (match.Success)
            {
                productName = match.Groups[1].Value;
            }

            var productRecord = records!.FirstOrDefault(p => p.Product.Equals(productName));
            _cartService.Add(callbackQuery.From.Id, productRecord!);

            return await BotClient.EditMessageTextAsync(
                chatId: callbackQuery.Message.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: $"{productName} успешно добавлен в вашу корзину!",
                replyMarkup: KeyboardHelper.CreateResetInlineKeyboard(),
                cancellationToken: cancellationToken);
        }
    }
}
