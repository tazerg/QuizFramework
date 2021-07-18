namespace QuizFramework.Notifications
{
    public interface INotificationDatabase
    {
        NotificationData? GetNotificationData(NotificationId notificationId);
    }
}