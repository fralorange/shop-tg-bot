using System.Collections.Concurrent;

namespace FreelanceBotBase.Bot.Services.State
{
    public class BotStateService : IBotStateService
    {
        private readonly ConcurrentDictionary<long, Domain.State.BotState> _botStates;

        public BotStateService()
            => _botStates = new ConcurrentDictionary<long, Domain.State.BotState>();


        public Domain.State.BotState GetOrCreateBotState(long chatId)
            => _botStates.GetOrAdd(chatId, _ => new Domain.State.BotState());
    }
}
