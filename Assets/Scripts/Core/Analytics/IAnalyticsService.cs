using System.Collections.Generic;

namespace QuizFramework.Analytics
{
    public interface IAnalyticsService
    {
        void SetUserId(string userId);
        void SendEvent(string eventId);
        void SendEvent(string eventId, IDictionary<string, object> eventArgs);
    }
}