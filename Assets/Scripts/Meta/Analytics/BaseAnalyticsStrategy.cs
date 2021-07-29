using System.Collections.Generic;
using QuizFramework.Storage;

namespace QuizFramework.Analytics
{
    public abstract class BaseAnalyticsStrategy
    {
        private readonly IDictionary<string, object> _globalArgs = new Dictionary<string, object>();
        
        protected IAnalyticsService AnalyticsService { get; }
        protected ILocalStorage LocalStorage { get; }

        protected BaseAnalyticsStrategy(IAnalyticsService analyticsService, ILocalStorage localStorage)
        {
            AnalyticsService = analyticsService;
            LocalStorage = localStorage;
        }
        
        protected IDictionary<string, object> GetGlobalArgs()
        {
            _globalArgs.Clear();

            _globalArgs.Add("playerId", LocalStorage.GetPlayerId());
            _globalArgs.Add("questionsDatabaseVersion", LocalStorage.GetLocalVersion());
            _globalArgs.Add("maxPassedLevel", LocalStorage.GetMaxPassedLevel());
            
            return _globalArgs;
        }
    }
}