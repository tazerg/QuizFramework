#if UNITY_ANDROID
using System;
using Unity.Notifications.Android;
using Zenject;

namespace QuizFramework.Notifications
{
    public class AndroidNotificationSender : INotificationSender, IInitializable
    {
        private const string NotificationChanelId = "jhi_quiz";
        private const string NotificationChanelName = "JHI Quiz";
        private const string NotificationChanelDescription = "JHI Quiz notification chanel";

        //private const string SmallIconId = "";
        //private const string LargeIconId = "";

        private void Initialize()
        {
            var channel = new AndroidNotificationChannel()
            {
                Id = NotificationChanelId,
                Name = NotificationChanelName,
                Description = NotificationChanelDescription,
                Importance = Importance.Default,
                LockScreenVisibility = LockScreenVisibility.Public,
                CanShowBadge = true,
                EnableVibration = true,
            };
            
            AndroidNotificationCenter.Initialize();
            AndroidNotificationCenter.RegisterNotificationChannel(channel);
        }
        
        private void SendNotification(NotificationDataDecorator notification)
        {
            var notificationToSend = new AndroidNotification
            {
                Title = notification.Title,
                Text = notification.Text,
                FireTime = DateTime.Now.Add(notification.TimeSpan),
                //SmallIcon = SmallIconId,
                //LargeIcon = LargeIconId,
                RepeatInterval = notification.RepeatInterval,
            };

            AndroidNotificationCenter.SendNotification(notificationToSend, NotificationChanelId);
        }

        private void RemoveAllNotifications()
        {
            AndroidNotificationCenter.CancelAllNotifications();
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

        #region IInitializable

        void IInitializable.Initialize()
        {
            Initialize();
        }

        #endregion
    }
}
#endif