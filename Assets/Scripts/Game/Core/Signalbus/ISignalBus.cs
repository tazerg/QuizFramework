using System;

namespace QuizFramework.SignalBus
{
    public interface ISignalBus
    {
        void Fire<T>(T obj);
        void Subscribe<T>(Action action);
        void Subscribe<T>(Action<T> action);
        void Unsubscribe<T>(Action action);
        void Unsubscribe<T>(Action<T> action);
    }
}