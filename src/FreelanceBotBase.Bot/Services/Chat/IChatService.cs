namespace FreelanceBotBase.Bot.Services.Chat
{
    /// <summary>
    /// Chat service to maintain chat connection between two users.
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// Connect users.
        /// </summary>
        /// <param name="user1Id">First user id.</param>
        /// <param name="user2Id">Second user id.</param>
        void ConnectUsers(long user1Id, long user2Id);
        /// <summary>
        /// Disconnect users.
        /// </summary>
        /// <param name="user1Id">First user id.</param>
        /// <param name="user2Id">Second user id.</param>
        void DisconnectUsers(long user1Id, long user2Id);
        /// <summary>
        /// Get connected users
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>User id.</returns>
        public long? GetConnectedUser(long userId);
    }
}
