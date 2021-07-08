using System;
using System.Collections.Generic;

namespace QuizFramework.SignalBus
{
    public class SubscribersList<T> : ISubscribersList
    {
        private readonly List<Action<T>> _genericActions = new List<Action<T>>();
        private readonly List<Action> _actions = new List<Action>();

        public void Add(Action<T> action)
        {
            if (_genericActions.Contains(action))
            {
                return;
            }
            
            _genericActions.Add(action);
        }

        public void Remove(Action<T> action)
        {
            if (!_genericActions.Contains(action))
            {
                return;
            }

            _genericActions.Remove(action);
        }

        private void Fire(object arg)
        {
            var genericArg = (T) arg;
            foreach (var genericAction in _genericActions)
            {
                genericAction.Invoke(genericArg);
            }
            
            foreach (var action in _actions)
            {
                action.Invoke();
            }
        }

        private void Add(Action action)
        {
            if (_actions.Contains(action))
            {
                return;
            }
            
            _actions.Add(action);
        }

        private void Remove(Action action)
        {
            if (!_actions.Contains(action))
            {
                return;
            }

            _actions.Remove(action);
        }

        #region ISubscribersList

        void ISubscribersList.Fire(object arg)
        {
            Fire(arg);
        }

        void ISubscribersList.Add(Action action)
        {
            Add(action);
        }

        void ISubscribersList.Remove(Action action)
        {
            Remove(action);
        }

        #endregion
    }
}