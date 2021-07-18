using System.Collections.Generic;
using Zenject;

namespace QuizFramework.Notifications
{
    public class NotificationsFactory : INotificationsFactory
    {
        private readonly DiContainer _container;

        public NotificationsFactory(DiContainer diContainer)
        {
            _container = diContainer;
        }
        
        private IEnumerable<INotification> CreateNotifications()
        {
            var notifications = new List<INotification>
            {
                _container.Instantiate<DayPassedNotification>(),
            };

            return notifications;
        }

        #region INotificationsFactory

        IEnumerable<INotification> INotificationsFactory.CreateNotifications()
        {
            return CreateNotifications();
        }

        #endregion
    }
}