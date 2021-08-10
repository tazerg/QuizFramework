using QuizFramework.Storage;
using Zenject;

namespace QuizFramework.Analytics
{
    public class PlayerAnalyticsStrategy : BaseAnalyticsStrategy, IPlayerAnalyticsStrategy, IInitializable
    {
        private const string QuizDatabaseUpdatedEventId = "quizDatabaseUpdated";
        
        public PlayerAnalyticsStrategy(IAnalyticsService analyticsService, ILocalStorage localStorage) : 
            base(analyticsService, localStorage)
        {
        }

        private void Initialize()
        {
            var playerId = LocalStorage.GetPlayerId();
            AnalyticsService.SetUserId(playerId);
        }

        private void QuizDatabaseVersionUpdatedEvent(int oldVersion, int newVersion)
        {
            var eventArgs = GetGlobalArgs();
            eventArgs.Add("oldVersion", oldVersion);
            eventArgs.Add("newVersion", newVersion);
            
            AnalyticsService.SendEvent(QuizDatabaseUpdatedEventId, eventArgs);
        }

        #region IPlayerAnalyticsStrategy

        void IPlayerAnalyticsStrategy.QuizDatabaseVersionUpdatedEvent(int oldVersion, int newVersion)
        {
            QuizDatabaseVersionUpdatedEvent(oldVersion, newVersion);
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