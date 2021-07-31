using System.Collections.Generic;
using UnityEngine;
using UnityAnalytics = UnityEngine.Analytics.Analytics;

namespace QuizFramework.Analytics
{
    public class UnityAnalyticsService : IAnalyticsService
    {
        private void SetUserId(string userId)
        {
            UnityAnalytics.enabled = true;
            var result = UnityAnalytics.SetUserId(userId);
            Debug.Log($"Send user id {userId} result {result.ToString()}");
        }

        private void SendEvent(string eventId, IDictionary<string, object> eventArgs = null)
        {
            if (eventArgs == null)
            {
                UnityAnalytics.CustomEvent(eventId);
                return;
            }

            var result = UnityAnalytics.CustomEvent(eventId, eventArgs);
            Debug.Log($"Send event {eventId} result {result.ToString()}");
        }
        
        #region IAnalyticsService

        void IAnalyticsService.SetUserId(string userId)
        {
            SetUserId(userId);
        }

        void IAnalyticsService.SendEvent(string eventId)
        {
            SendEvent(eventId);
        }

        void IAnalyticsService.SendEvent(string eventId, IDictionary<string, object> eventArgs)
        {
            SendEvent(eventId, eventArgs);
        }

        #endregion
    }
}