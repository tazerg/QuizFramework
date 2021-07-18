using System;

namespace QuizFramework.Notifications
{
    public struct ScheduledNotification
    {
        public NotificationId NotificationId { get; }
        public DateTime ScheduleTime { get; }

        public ScheduledNotification(NotificationId notificationId, DateTime scheduleTime)
        {
            NotificationId = notificationId;
            ScheduleTime = scheduleTime;
        }
    }
}