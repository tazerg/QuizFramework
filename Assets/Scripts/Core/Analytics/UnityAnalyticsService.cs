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
            UnityAnalytics.SetUserId(userId);
        }

        private void SendEvent(string eventId, IDictionary<string, object> eventArgs = null)
        {
            if (eventArgs == null)
            {
                UnityAnalytics.CustomEvent(eventId);
                return;
            }

            UnityAnalytics.CustomEvent(eventId, eventArgs);
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