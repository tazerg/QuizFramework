using QuizFramework.Storage;
using Zenject;

namespace QuizFramework.Analytics
{
    public class PlayerAnalyticsStrategy : BaseAnalyticsStrategy, IInitializable
    {
        public PlayerAnalyticsStrategy(IAnalyticsService analyticsService, ILocalStorage localStorage) : 
            base(analyticsService, localStorage)
        {
        }

        private void Initialize()
        {
            var playerId = LocalStorage.GetPlayerId();
            AnalyticsService.SetUserId(playerId);
        }

        #region IInitializable

        void IInitializable.Initialize()
        {
            Initialize();
        }

        #endregion
    }
}