namespace QuizFramework.Notifications
{
    public interface INotificationSender
    {
        void SendNotification(NotificationDataDecorator notificationData);
        void RemoveAllNotifications();
    }
}