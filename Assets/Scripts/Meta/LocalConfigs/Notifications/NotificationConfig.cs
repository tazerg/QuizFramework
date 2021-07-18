using QuizFramework.Notifications;
using UnityEngine;

namespace QuizFramework.LocalConfig
{
    [CreateAssetMenu(fileName = "NotificationConfig", menuName = "Quiz/Notification Config")]
    public class NotificationConfig : ScriptableObject, INotificationConfig
    {
        [SerializeField] private int _minimumTimeInterval;
        [SerializeField] private int _minimumHourOfSend;
        [SerializeField] private int _maximumHourOfSend;

        #region INotificationsConfig

        int INotificationConfig.MinimumTimeInterval => _minimumTimeInterval;
        int INotificationConfig.MinimumHourOfSend => _minimumHourOfSend;
        int INotificationConfig.MaximumHourOfSend => _maximumHourOfSend;

        #endregion
    }
}