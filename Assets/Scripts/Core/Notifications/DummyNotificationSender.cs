#if UNITY_EDITOR
using UnityEngine;

namespace QuizFramework.Notifications
{
    public class DummyNotificationSender : INotificationSender
    {
        private void SendNotification(NotificationDataDecorator notification)
        {
            Debug.Log($"Send notification {notification.Text}");
        }

        private void RemoveAllNotifications()
        {
            Debug.Log($"Remove notifications");
        }

        #region INotificationSender

        void INotificationSender.SendNotification(NotificationDataDecorator notification)
        {
            SendNotification(notification);
        }

        void INotificationSender.RemoveAllNotifications()
        {
            RemoveAllNotifications();
        }

        #endregion
    }
}
#endif