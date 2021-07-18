using System.Collections.Generic;
using UnityEngine;

namespace QuizFramework.Notifications
{
    public class NotificationDatabase : INotificationDatabase
    {
        private readonly Dictionary<NotificationId, NotificationData> _notificationDatabase =
            new Dictionary<NotificationId, NotificationData>();

        public NotificationDatabase(IEnumerable<NotificationData> notifications)
        {
            foreach (var notification in notifications)
            {
                if (_notificationDatabase.ContainsKey(notification.Id))
                {
                    Debug.LogError($"Notification {notification.Id} already exist in database!");
                    continue;
                }
                
                _notificationDatabase.Add(notification.Id, notification);
            }
        }

        private NotificationData? GetNotificationData(NotificationId notificationId)
        {
            if (!_notificationDatabase.TryGetValue(notificationId, out var notification))
            {
                Debug.LogError($"Can't find notification with id {notificationId}");
                return null;
            }

            return notification;
        }

        #region INotificationDatabase

        NotificationData? INotificationDatabase.GetNotificationData(NotificationId notificationId)
        {
            return GetNotificationData(notificationId);
        }

        #endregion
    }
}