using AutoMapper;
using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Commands.Forbidden;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Contracts.User;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Text.GetUsers
{
    /// <summary>
    /// Command that sends sequence of users to owner.
    /// </summary>
    public class GetUsersCommand : CommandBase
    {
        /// <summary>
        /// User repository.
        /// </summary>
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates get users command.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        public GetUsersCommand(ITelegramBotClient botClient, IUserRepository userRepository, IMapper mapper) : base(botClient)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public override async Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var currentUser = await _userRepository.GetByIdAsync(message.From!.Id);
            if (currentUser == null || currentUser.UserRole != Domain.User.User.Role.Owner)
                return await ForbiddenOutputFromCommand.ForbidAsync(BotClient, message, cancellationToken);

            var users = await _userRepository.GetAll();
            var usersDto = _mapper.Map<List<UserDto>>(users);

            var output = PaginationHelper.Format(usersDto);
            var inlineKeyboard = KeyboardHelper.CreateGetUsersInlineKeyboard();

            return await BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: output,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
