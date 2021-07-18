using System;
using UnityEngine;

namespace QuizFramework.Notifications
{
    public abstract class BaseNotification : INotification
    {
        private const double MinSecondsToSendNotification = 1;

        private readonly INotificationController _notificationController;

        protected BaseNotification(INotificationController notificationController)
        {
            _notificationController = notificationController;
        }
        
        protected abstract NotificationId NotificationId { get; }
        protected abstract bool UseCustomTimeSpan { get; }
        
        protected abstract bool CanSendNotification();
        protected abstract TimeSpan GetCustomTimeSpan();

        private void TrySendNotification()
        {
            if (!CanSendNotification())
            {
                return;
            }

            if (UseCustomTimeSpan)
            {
                var timeSpan = GetCustomTimeSpan();
                if (timeSpan.TotalSeconds <= MinSecondsToSendNotification)
                {
                    Debug.LogError($"Trying to send notification {NotificationId} with wrong timespan: {timeSpan}");
                    return;
                }
				
                _notificationController.SendNotification(NotificationId, timeSpan);
                return;
            }

            _notificationController.SendNotification(NotificationId);
        }

        #region INotification

        void INotification.TrySendNotification()
        {
            
        }

        #endregion
    }
}