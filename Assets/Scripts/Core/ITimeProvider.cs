using System;

namespace QuizFramework.Core
{
    public interface ITimeProvider
    {
        DateTime UtcTime { get; }

        DateTime LocalTime { get; }

        DateTime ConvertUtcToLocalTime(DateTime utcTime);

        DateTime ConvertLocalToUtcTime(DateTime localTime);
    }
}