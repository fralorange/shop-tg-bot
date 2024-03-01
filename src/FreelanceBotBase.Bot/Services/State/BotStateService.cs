using System.Collections.Concurrent;

namespace FreelanceBotBase.Bot.Services.State
{
    /// <inheritdoc cref="IBotStateService"/>
    public class BotStateService : IBotStateService
    {
        /// <summary>
        /// Bot states for <see cref="long"/> user ids
        /// </summary>
        private readonly ConcurrentDictionary<long, Domain.State.BotState> _botStates;

        /// <summary>
        /// Initializes new bot state service.
        /// </summary>
        public BotStateService()
            => _botStates = new ConcurrentDictionary<long, Domain.State.BotState>();


        public Domain.State.BotState GetOrCreateBotState(long chatId)
            => _botStates.GetOrAdd(chatId, _ => new Domain.State.BotState());
    }
}
