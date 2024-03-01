namespace FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Facades
{
    /// <summary>
    /// User facade to interpet system external methods.
    /// </summary>
    public interface IUserFacade
    {
        /// <summary>
        /// Asigns delivery point to manager.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deliveryPointId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AssignDeliveryPointAsync(long userId, long? deliveryPointId, CancellationToken cancellationToken);
    }
}
