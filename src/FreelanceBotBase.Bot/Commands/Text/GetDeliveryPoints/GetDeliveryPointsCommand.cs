using AutoMapper;
using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Commands.Forbidden;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Contracts.DeliveryPoint;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Repositories;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Text.GetDeliveryPoints
{
    /// <summary>
    /// Command that sends sequence of delivery points to manager.
    /// </summary>
    public class GetDeliveryPointsCommand : CommandBase
    {
        /// <summary>
        /// Delivery point repository.
        /// </summary>
        private readonly IDeliveryPointRepository _deliveryPointRepository;
        /// <summary>
        /// User repository.
        /// </summary>
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates get delivery point command.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="deliveryPointRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
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
