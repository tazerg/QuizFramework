using System;
using Zenject;

namespace QuizFramework.Core
{
    public class ClientTimeProvider : ITimeProvider, IInitializable
    {
        private TimeSpan _utcTimeOffset = TimeSpan.Zero;

        private DateTime UtcTime => DateTime.UtcNow;

        private DateTime LocalTime => DateTime.Now;

        private void Initialize()
        {
            _utcTimeOffset = DateTime.Now - DateTime.UtcNow;
        }

        private DateTime ConvertLocalToUtcTime(DateTime localTime)
        {
            return localTime.Add(-_utcTimeOffset);
        }

        private DateTime ConvertUtcToLocalTime(DateTime utcTime)
        {
            return utcTime.Add(_utcTimeOffset);
        }

        #region ITimeProvider

        DateTime ITimeProvider.UtcTime => UtcTime;

        DateTime ITimeProvider.LocalTime => LocalTime;

        DateTime ITimeProvider.ConvertLocalToUtcTime(DateTime localTime)
        {
            return ConvertLocalToUtcTime(localTime);
        }

        DateTime ITimeProvider.ConvertUtcToLocalTime(DateTime utcTime)
        {
            return ConvertUtcToLocalTime(utcTime);
        }

        #endregion

        #region IInitializable

        void IInitializable.Initialize()
        {
            Initialize();
        }

        #endregion
    }
}