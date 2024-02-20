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
            Delete,
            Selection,
            ChoosingDeliveryPoint,
        }

        public State CurrentState { get; set; } = State.Default;
        public InputState AwaitingInputState { get; set; } = InputState.None;
    }
}
