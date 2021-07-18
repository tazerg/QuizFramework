using System.Collections.Generic;

namespace QuizFramework.Notifications
{
    public interface INotificationsFactory
    {
        IEnumerable<INotification> CreateNotifications();
    }
}