using System.Collections.Concurrent;

namespace FreelanceBotBase.Bot.Services.Chat
{
    /// <inheritdoc cref="IChatService"/>
    public class ChatService : IChatService
    {
        private readonly ConcurrentDictionary<long, long> _userChats;

        /// <summary>
        /// Initializes new chat service.
        /// </summary>
        public ChatService()
            => _userChats = new ConcurrentDictionary<long, long>();

        public void ConnectUsers(long user1Id, long user2Id)
        {
            _userChats.TryAdd(user1Id, user2Id);
            _userChats.TryAdd(user2Id, user1Id);
        }

        public void DisconnectUsers(long user1Id, long user2Id)
        {
            _userChats.TryRemove(user1Id, out _);
            _userChats.TryRemove(user2Id, out _);
        }

        public long? GetConnectedUser(long userId)
        {
            if (_userChats.TryGetValue(userId, out var connectedUserId))
                return connectedUserId;
            return null;
        }
    }
}
