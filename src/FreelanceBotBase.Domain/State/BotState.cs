namespace FreelanceBotBase.Domain.State
{
    public class BotState
    {
        public enum State
        {
            Default,
            AwaitingInput,
            Chatting,
        }

        public enum InputState
        {
            None,
            Search,
            Delete,
            Selection,
            ChoosingDeliveryPoint,
            EditingDpName,
            EditingDpLocation,
            AddingUser,
            RemovingUser,
            AsigningDpManager
        }

        public State CurrentState { get; set; } = State.Default;
        public InputState AwaitingInputState { get; set; } = InputState.None;
    }
}
