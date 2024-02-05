namespace FreelanceBotBase.Domain.States
{
    public class BotState
    {
        public enum State
        {
            Default,
            AwaitingInput,
        }

        public enum InputState
        {
            None,
            Search,
            Selection,
        }

        public State CurrentState { get; set; } = State.Default;
        public InputState AwaitingInputState { get; set; } = InputState.None;
        //if this attributes will be used only for search and never again then remove them so it won't ruin SRP.
        public long ChatId { get; set; }
    }
}
