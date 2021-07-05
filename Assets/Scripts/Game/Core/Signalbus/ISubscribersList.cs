using System;

namespace QuizFramework.SignalBus
{
    public interface ISubscribersList
    {
        void Fire(object arg);
        void Add(Action action);
        void Remove(Action action);
    }
}