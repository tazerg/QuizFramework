namespace QuizFramework.Notifications
{
    public interface INotificationConfig
    {
        int MinimumTimeInterval { get; }
        int MinimumHourOfSend { get; }
        int MaximumHourOfSend { get; }
    }
}