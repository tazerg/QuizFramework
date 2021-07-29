namespace QuizFramework.Analytics
{
    public interface IRedirectAnalyticsStrategy
    {
        void ReportOpenSocialNetworkEvent(string socialNetwork);
        void ReportOpenMoreGamesEvent();
    }
}