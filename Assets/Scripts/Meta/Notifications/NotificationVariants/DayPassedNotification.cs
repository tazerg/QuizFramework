using System;

namespace QuizFramework.Notifications
{
    public class DayPassedNotification : BaseNotification
    {
        public DayPassedNotification(INotificationController notificationController) : base(notificationController)
        {
        }

        protected override NotificationId NotificationId => NotificationId.DayPassed;
        protected override bool UseCustomTimeSpan => false;
        
        protected override bool CanSendNotification()
        {
            return true;
        }

        protected override TimeSpan GetCustomTimeSpan()
        {
            throw new NotImplementedException();
        }
    }
}