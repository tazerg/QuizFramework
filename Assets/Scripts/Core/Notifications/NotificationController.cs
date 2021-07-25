using System;
using System.Collections.Generic;
using System.Linq;
using QuizFramework.Core;

namespace QuizFramework.Notifications
{
    public class NotificationController : INotificationController
    {
        private readonly INotificationConfig _notificationConfig;
        private readonly INotificationDatabase _notificationDatabase;
        private readonly INotificationSender _notificationSender;
        private readonly ITimeProvider _timeProvider;

        private readonly HashSet<ScheduledNotification> _scheduledNotifications = new HashSet<ScheduledNotification>();

        public NotificationController(INotificationConfig notificationConfig, INotificationDatabase notificationDatabase,
            INotificationSender notificationSender, ITimeProvider timeProvider)
        {
            _notificationConfig = notificationConfig;
            _notificationDatabase = notificationDatabase;
            _notificationSender = notificationSender;
            _timeProvider = timeProvider;
        }

        private void SendNotification(NotificationId notificationId, TimeSpan customTimeSpan = default)
        {
            var notification = _notificationDatabase.GetNotificationData(notificationId);
            if (notification == null)
            {
                return;
            }
            
            var timeSpan = customTimeSpan != default ? customTimeSpan : notification.Value.TimeSpan;
            var notificationDecorator = CreateNotificationDecorator(notification.Value, timeSpan);
            _notificationSender.SendNotification(notificationDecorator);
        }

        private void RemoveAllNotifications()
        {
            _scheduledNotifications.Clear();
            _notificationSender.RemoveAllNotifications();
        }
        
        private bool IsNotificationSent(NotificationId notificationId)
        {
            var scheduledNotification = _scheduledNotifications.FirstOrDefault(x => x.NotificationId == notificationId);
            return scheduledNotification.NotificationId == notificationId && scheduledNotification.ScheduleTime > _timeProvider.LocalTime;
        }

        private DateTime GetNotificationScheduleTime(NotificationId notificationId)
        {
            var scheduledNotification = _scheduledNotifications.FirstOrDefault(x => x.NotificationId == notificationId);
            return scheduledNotification.ScheduleTime;
        }
        
        private NotificationDataDecorator CreateNotificationDecorator(NotificationData notification, TimeSpan timeSpan)
        {
            var sendTime = _timeProvider.LocalTime.Add(timeSpan);
            sendTime = GetValidSendTime(sendTime);

            _scheduledNotifications.Add(new ScheduledNotification(notification.Id, sendTime));

            return new NotificationDataDecorator(notification, sendTime.Subtract(_timeProvider.LocalTime));
        }

        private DateTime GetValidSendTime(DateTime sendTime)
        {
            if (IsSendTimeFellAtNight(sendTime))
            {
                if (IsSendTimeBeforeMidnight(sendTime))
                {
                    sendTime = sendTime.AddDays(1);
                }

                sendTime = new DateTime(sendTime.Year, sendTime.Month, sendTime.Day,
                    _notificationConfig.MinimumHourOfSend, sendTime.Minute, sendTime.Second);
            }

            if (_scheduledNotifications.Count == 0)
            {
                return sendTime;
            }

            sendTime = RecursivelyFindClosestValidTime(sendTime);
            return sendTime;
        }

        private bool IsSendTimeFellAtNight(DateTime sendTime)
        {
            var sendHour = sendTime.Hour;
            return sendHour > _notificationConfig.MaximumHourOfSend || sendHour < _notificationConfig.MinimumHourOfSend;
        }

        private bool IsSendTimeBeforeMidnight(DateTime sendTime)
        {
            var sendHour = sendTime.Hour;
            return sendHour > _notificationConfig.MaximumHourOfSend;
        }

        private DateTime RecursivelyFindClosestValidTime(DateTime sourceTime)
        {
            var nearestNotification = _scheduledNotifications.FirstOrDefault(x => x.ScheduleTime.Subtract(sourceTime).Duration().TotalSeconds < _notificationConfig.MinimumTimeInterval);
            if (nearestNotification.NotificationId == NotificationId.None)
            {
                return sourceTime;
            }

            sourceTime = nearestNotification.ScheduleTime.AddSeconds(_notificationConfig.MinimumTimeInterval);
            return RecursivelyFindClosestValidTime(sourceTime);
        }

        #region INotificationController

        void INotificationController.SendNotification(NotificationId notificationId)
        {
            SendNotification(notificationId);
        }

        void INotificationController.SendNotification(NotificationId notificationId, TimeSpan customTimeSpan)
        {
            SendNotification(notificationId, customTimeSpan);
        }

        void INotificationController.RemoveAllNotifications()
        {
            RemoveAllNotifications();
        }

        bool INotificationController.IsNotificationSent(NotificationId notificationId)
        {
            return IsNotificationSent(notificationId);
        }

        DateTime INotificationController.GetNotificationScheduleTime(NotificationId notificationId)
        {
            return GetNotificationScheduleTime(notificationId);
        }

        #endregion
    }
}