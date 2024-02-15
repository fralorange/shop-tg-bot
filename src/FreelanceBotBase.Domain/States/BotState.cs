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
    }
}
