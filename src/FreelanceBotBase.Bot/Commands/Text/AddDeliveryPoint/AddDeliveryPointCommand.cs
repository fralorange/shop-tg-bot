using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Commands.Forbidden;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Text.AddDeliveryPoint
{
    public class AddDeliveryPointCommand : CommandBase
    {
        private readonly IUserRepository _userRepository;

        public AddDeliveryPointCommand(ITelegramBotClient botClient, IUserRepository userRepository) : base(botClient)
            => _userRepository = userRepository;

        public override async Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var currentUser = await _userRepository.GetByIdAsync(message.From!.Id);
            if (currentUser.UserRole != Domain.User.User.Role.Owner)
                return await ForbiddenOutputFromCommand.ForbidAsync(BotClient, message, cancellationToken);

            return await BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Название: ...\nЛокация: ...\n",
                replyMarkup: InlineKeyboardHelper.CreateAddNewDpInlineKeyboard(),
                cancellationToken: cancellationToken);
        }
    }
}
