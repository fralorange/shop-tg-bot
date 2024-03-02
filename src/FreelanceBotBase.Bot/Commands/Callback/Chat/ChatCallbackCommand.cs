using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Bot.Services.Cart;
using FreelanceBotBase.Bot.Services.Chat;
using FreelanceBotBase.Bot.Services.State;
using FreelanceBotBase.Domain.State;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Callback.Chat
{
    /// <summary>
    /// Callback command that creates chat connection between manager and user.
    /// </summary>
    public class ChatCallbackCommand : CallbackCommandBase
    {
        private readonly IChatService _chatService;
        private readonly IBotStateService _botStateService;
        private readonly ICartService _cartService;
        /// <summary>
        /// Creates chat callback command.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="chatService"></param>
        /// <param name="botStateService"></param>
        public ChatCallbackCommand(ITelegramBotClient botClient, IChatService chatService, IBotStateService botStateService, ICartService cartService) : base(botClient)
        {
            _chatService = chatService;
            _botStateService = botStateService;
            _cartService = cartService;
        }

        public override async Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var text = callbackQuery.Message!.Text!;
            var userId = Convert.ToInt64(Regex.Match(text, "(ID: )(.*)(,)").Groups[2].Value);
            var managerId = callbackQuery.From.Id;

            var userChatBotState = _botStateService.GetOrCreateBotState(userId);
            var managerChatBotState = _botStateService.GetOrCreateBotState(managerId);

            userChatBotState.CurrentState = BotState.State.Chatting;
            managerChatBotState.CurrentState = BotState.State.Chatting;

            _chatService.ConnectUsers(userId, managerId);

            await BotClient.SendTextMessageAsync(
                chatId: userId,
                text: $"Чат успешно подключен с менеджером по ID: {managerId}",
                replyMarkup: KeyboardHelper.CreateChatReplyKeyboard(),
                cancellationToken: cancellationToken);

            var products = _cartService.Get(userId);
            var formattedOutput = PaginationHelper.Format(products!);

            return await BotClient.SendTextMessageAsync(
                chatId: managerId,
                text: $"Чат успешно подключен с пользователем по ID: {userId}, заказавшим:\n" + formattedOutput,
                replyMarkup: KeyboardHelper.CreateChatReplyKeyboard(),
                cancellationToken: cancellationToken);
        }
    }
}
