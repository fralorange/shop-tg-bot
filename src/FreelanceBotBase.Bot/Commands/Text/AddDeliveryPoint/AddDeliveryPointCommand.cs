using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Text.AddDeliveryPoint
{
    public class AddDeliveryPointCommand : CommandBase
    {
        private readonly IDeliveryPointRepository _deliveryPointRepository;

        public AddDeliveryPointCommand(ITelegramBotClient botClient, IDeliveryPointRepository deliveryPointRepository) : base(botClient)
            => _deliveryPointRepository = deliveryPointRepository;
        

        public override Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
