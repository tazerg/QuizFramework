using System;

namespace QuizFramework.Notifications
{
    public interface INotificationController
    {
        void SendNotification(NotificationId notificationId);
        void SendNotification(NotificationId notificationId, TimeSpan customTimeSpan);
        void RemoveAllNotifications();
        bool IsNotificationSent(NotificationId notificationId);
        DateTime GetNotificationScheduleTime(NotificationId notificationId);
    }
}