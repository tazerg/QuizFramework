using System;

namespace QuizFramework.Notifications
{
    public struct NotificationData
    {
        public NotificationId Id { get; }
        public string Title { get; }
        public string Text { get; }
        public TimeSpan TimeSpan { get; }
        public TimeSpan? RepeatInterval { get; }

        public NotificationData(NotificationId id, string title, string text, TimeSpan timeSpan, TimeSpan? repeatInterval)
        {
            Id = id;
            Title = title;
            Text = text;
            TimeSpan = timeSpan;
            RepeatInterval = repeatInterval;
        }
    }
}