using State = FreelanceBotBase.Domain.States.BotState;

namespace FreelanceBotBase.Bot.Services.BotState
{
    public interface IBotStateService
    {
        State GetOrCreateBotState(long chatId); // if bot will be used in groups then it will need to use userId with chatId
    }
}
