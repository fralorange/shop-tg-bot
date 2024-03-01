namespace FreelanceBotBase.Bot.Services.State
{
    /// <summary>
    /// Bot state service to regulate different states between different users.
    /// </summary>
    public interface IBotStateService
    {
        /// <summary>
        /// Gets or creates bot state.
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        Domain.State.BotState GetOrCreateBotState(long chatId); // if bot will be used in groups then it will need to use userId with chatId
    }
}
