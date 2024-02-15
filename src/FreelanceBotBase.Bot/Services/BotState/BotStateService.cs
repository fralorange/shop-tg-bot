using System.Collections.Concurrent;
using State = FreelanceBotBase.Domain.States.BotState;

namespace FreelanceBotBase.Bot.Services.BotState
{
    public class BotStateService : IBotStateService
    {
        private readonly ConcurrentDictionary<long, State> _botStates;

        public BotStateService()
            => _botStates = new ConcurrentDictionary<long, State>();


        public State GetOrCreateBotState(long chatId)
            => _botStates.GetOrAdd(chatId, _ => new State());
    }
}
