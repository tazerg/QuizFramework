using System;
using System.Collections.Generic;

namespace QuizFramework.SignalBus
{
    public class SignalBus : ISignalBus
    {
        private readonly IDictionary<Type, ISubscribersList> _subscribers = new Dictionary<Type, ISubscribersList>();

        private void Fire<T>(T obj)
        {
            var type = typeof(T);
            if (!_subscribers.TryGetValue(type, out var subscribersList))
            {
                return;
            }
            
            subscribersList.Fire(obj);
        }

        private void Subscribe<T>(Action action)
        {
            var subscribersList = GetSubscribersList<T>();
            subscribersList.Add(action);
        }

        private void Subscribe<T>(Action<T> action)
        {
            var subscribersList = (SubscribersList<T>) GetSubscribersList<T>();
            subscribersList.Add(action);
        }

        private void Unsubscribe<T>(Action action)
        {
            var subscribersList = GetSubscribersList<T>();
            subscribersList.Remove(action);
        }

        private void Unsubscribe<T>(Action<T> action)
        {
            var subscribersList = (SubscribersList<T>) GetSubscribersList<T>();
            subscribersList.Remove(action);
        }

        private ISubscribersList GetSubscribersList<T>()
        {
            var type = typeof(T);
            if (_subscribers.TryGetValue(type, out var subscribersList)) 
                return subscribersList;
            
            subscribersList = new SubscribersList<T>();
            _subscribers.Add(type, subscribersList);
            return subscribersList;
        }

        #region ISignalBus

        void ISignalBus.Fire<T>(T obj)
        {
            Fire(obj);
        }

        void ISignalBus.Subscribe<T>(Action action)
        {
            Subscribe<T>(action);
        }

        void ISignalBus.Subscribe<T>(Action<T> action)
        {
            Subscribe(action);
        }

        void ISignalBus.Unsubscribe<T>(Action action)
        {
            Unsubscribe<T>(action);
        }

        void ISignalBus.Unsubscribe<T>(Action<T> action)
        {
            Unsubscribe(action);
        }

        #endregion
    }
}