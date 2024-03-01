namespace FreelanceBotBase.Domain.State
{
    public class BotState
    {
        /// <summary>
        /// Bot main state.
        /// </summary>
        public enum State
        {
            Default,
            AwaitingInput,
            Chatting,
        }

        /// <summary>
        /// Bot input state.
        /// </summary>
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

        /// <summary>
        /// Bot main current state.
        /// </summary>
        public State CurrentState { get; set; } = State.Default;
        /// <summary>
        /// Bot input current state.
        /// </summary>
        public InputState AwaitingInputState { get; set; } = InputState.None;
    }
}
