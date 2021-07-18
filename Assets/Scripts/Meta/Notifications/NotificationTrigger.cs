using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace QuizFramework.Notifications
{
    public class NotificationTrigger : MonoBehaviour
    {
        [Inject] private INotificationController NotificationController { get; set; }
        [Inject] private INotificationsFactory NotificationsFactory { get; set; }

        private IEnumerable<INotification> _notifications;

        private void Awake()
        {
            NotificationController.RemoveAllNotifications();

            _notifications = NotificationsFactory.CreateNotifications();
        }
        
        private void OnApplicationPause(bool pause)
        {
            if (NotificationController == null)
            {
                return;
            }

            if (!pause)
            {
                NotificationController.RemoveAllNotifications();
                return;
            }

            SendActualNotifications();
        }

        private void SendActualNotifications()
        {
            foreach(var notification in _notifications)
            {
                notification.TrySendNotification();
            }
        }
    }
}