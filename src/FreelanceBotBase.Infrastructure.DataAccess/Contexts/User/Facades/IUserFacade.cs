namespace FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Facades
{
    public interface IUserFacade
    {
        Task AssignDeliveryPointAsync(long userId, long? deliveryPointId, CancellationToken cancellationToken);
    }
}
