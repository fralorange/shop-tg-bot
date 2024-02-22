using AutoMapper;
using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Contracts.User;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Text.GetUsers
{
    public class GetUsersCommand : CommandBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersCommand(ITelegramBotClient botClient, IUserRepository userRepository, IMapper mapper) : base(botClient)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public override async Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var currentUser = await _userRepository.GetByIdAsync(message.From!.Id);
            if (currentUser.UserRole != Domain.User.User.Role.Owner)
                return await BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Нет доступа к данной команде!",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);

            var users = await _userRepository.GetAll();
            var usersDto = _mapper.Map<List<UserDto>>(users);

            var output = PaginationHelper.Format(usersDto);
            var inlineKeyboard = InlineKeyboardHelper.CreateGetUsersInlineKeyboard();

            return await BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: output,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
