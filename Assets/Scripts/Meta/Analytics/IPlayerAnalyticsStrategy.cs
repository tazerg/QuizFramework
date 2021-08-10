namespace QuizFramework.Analytics
{
    public interface IPlayerAnalyticsStrategy
    {
        void QuizDatabaseVersionUpdatedEvent(int oldVersion, int newVersion);
    }
}