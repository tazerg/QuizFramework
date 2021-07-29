using QuizFramework.Storage;

namespace QuizFramework.Analytics
{
    public class RedirectAnalyticsStrategy : BaseAnalyticsStrategy, IRedirectAnalyticsStrategy
    {
        private const string OpenSocialEventId = "openSocialNetwork";
        private const string OpenMoreGamesEventId = "openMoreGames";
        
        public RedirectAnalyticsStrategy(IAnalyticsService analyticsService, ILocalStorage localStorage) : 
            base(analyticsService, localStorage)
        {
        }

        private void ReportOpenSocialNetworkEvent(string socialNetwork)
        {
            var eventArgs = GetGlobalArgs();
            eventArgs.Add("socialNetwork", socialNetwork);
            
            AnalyticsService.SendEvent(OpenSocialEventId, eventArgs);
        }

        private void ReportOpenMoreGamesEvent()
        {
            var eventArgs = GetGlobalArgs();
            
            AnalyticsService.SendEvent(OpenMoreGamesEventId, eventArgs);
        }

        #region IRedirectAnalyticsStrategy

        void IRedirectAnalyticsStrategy.ReportOpenSocialNetworkEvent(string socialNetwork)
        {
            ReportOpenSocialNetworkEvent(socialNetwork);
        }

        void IRedirectAnalyticsStrategy.ReportOpenMoreGamesEvent()
        {
            ReportOpenMoreGamesEvent();
        }

        #endregion
    }
}