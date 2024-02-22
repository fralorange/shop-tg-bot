namespace FreelanceBotBase.Domain.User
{
    /// <summary>
    /// User.
    /// </summary>
    public class User
    {
        /// <summary>
        /// User roles.
        /// </summary>
        public enum Role
        {
            /// <summary>
            /// Delivery point manager's role.
            /// </summary>
            Manager,
            /// <summary>
            /// Owner's role.
            /// </summary>
            Owner
        }

        /// <summary>
        /// User id.
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// User role.
        /// </summary>
        public Role UserRole { get; set; }
        //refactor this l8r cuz it breaks SRP when user aren't manager or owner.
        // just got a nice though, why the heck i would ever store users in database that's so stupid, i can only story admins and thats'all
        //refactor l8r!
        /// <summary>
        /// Delivery point id in which Admin takes place in.
        /// </summary>
        public long? DeliveryPointId { get; set; }
        /// <summary>
        /// Navigation variable for DeliveryPoint.
        /// </summary>
        public virtual DeliveryPoint.DeliveryPoint? DeliveryPoint { get; set; }
    }
}
