using AutoMapper;
using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Commands.Forbidden;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Contracts.DeliveryPoint;
using FreelanceBotBase.Domain.DeliveryPoint;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Repositories;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Text.GetDeliveryPoints
{
    public class GetDeliveryPointsCommand : CommandBase
    {
        private readonly IDeliveryPointRepository _deliveryPointRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetDeliveryPointsCommand(
            ITelegramBotClient botClient, IDeliveryPointRepository deliveryPointRepository,
            IUserRepository userRepository,
            IMapper mapper) : base(botClient)
        {
            _deliveryPointRepository = deliveryPointRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async override Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var currentUser = await _userRepository.GetByIdAsync(message.From!.Id);
            if (currentUser == null)
                return await ForbiddenOutputFromCommand.ForbidAsync(BotClient, message, cancellationToken);
            
            var deliveryPoints = await _deliveryPointRepository.GetAll();
            var deliveryPointsDto = _mapper.Map<List<DeliveryPointDto>>(deliveryPoints);

            var output = PaginationHelper.Format(deliveryPointsDto);
            var inlineKeyboard = InlineKeyboardHelper.CreateGetDeliveryPointsInlineKeyboard();

            return await BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: output,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
