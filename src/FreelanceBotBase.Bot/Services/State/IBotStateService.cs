namespace FreelanceBotBase.Bot.Services.State
{
    public interface IBotStateService
    {
        Domain.State.BotState GetOrCreateBotState(long chatId); // if bot will be used in groups then it will need to use userId with chatId
    }
}
