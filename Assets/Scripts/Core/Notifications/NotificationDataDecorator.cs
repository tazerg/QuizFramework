using System;

namespace QuizFramework.Notifications
{
    public struct NotificationDataDecorator
    {
        public string Title { get; }
        public string Text { get; }
        public TimeSpan TimeSpan { get; }
        public TimeSpan? RepeatInterval { get; }

        public NotificationDataDecorator(NotificationData data, TimeSpan sendTimeSpan)
        {
            Title = data.Title;
            Text = data.Text;
            TimeSpan = sendTimeSpan;
            RepeatInterval = data.RepeatInterval;
        }
    }
}