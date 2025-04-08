namespace Notification.Application.Common.Interfaces
{
    public interface INotificationService<in T>
    {
        Task Send(T message);
    }
}
