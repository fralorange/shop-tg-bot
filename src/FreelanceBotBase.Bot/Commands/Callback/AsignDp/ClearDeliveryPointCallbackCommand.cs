using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Facades;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Facades;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Callback.AsignDp
{
    public class ClearDeliveryPointCallbackCommand : CallbackCommandBase
    {
        private readonly IDeliveryPointFacade _deliveryPointFacade;
        private readonly IUserFacade _userFacade;
        private readonly IUserRepository _userRepository;

        public ClearDeliveryPointCallbackCommand(ITelegramBotClient botClient, IDeliveryPointFacade deliveryPointFacade, IUserFacade userFacade, IUserRepository userRepository)
            : base(botClient)
        {
            _deliveryPointFacade = deliveryPointFacade;
            _userFacade = userFacade;
            _userRepository = userRepository;
        }

        public async override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var userId = callbackQuery.From!.Id;
            var user = await _userRepository.GetByIdAsync(userId);
            var dpId = user.DeliveryPointId;
            if (dpId != null)
                await _deliveryPointFacade.SetManagerAsync(dpId.Value, null, cancellationToken);
            await _userFacade.AssignDeliveryPointAsync(userId, null, cancellationToken);

            return await BotClient.SendTextMessageAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                text: "Связь очищена!",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    }
}
