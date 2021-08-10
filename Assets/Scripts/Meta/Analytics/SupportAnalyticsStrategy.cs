using QuizFramework.Storage;

namespace QuizFramework.Analytics
{
    public class SupportAnalyticsStrategy : BaseAnalyticsStrategy, ISupportAnalyticsStrategy
    {
        private const string OpenSupportWindowEventId = "openSupportWindow";
        private const string StartBuyingProductEventId = "startBuying";
        
        public SupportAnalyticsStrategy(IAnalyticsService analyticsService, ILocalStorage localStorage) : 
            base(analyticsService, localStorage)
        {
        }

        private void OpenSupportWindowEvent()
        {
            var eventArgs = GetGlobalArgs();
            
            AnalyticsService.SendEvent(OpenSupportWindowEventId, eventArgs);
        }

        private void StartBuyingProductEvent(string productId, string productName)
        {
            var eventArgs = GetGlobalArgs();
            eventArgs.Add("productId", productId);
            eventArgs.Add("productName", productName);
            
            AnalyticsService.SendEvent(StartBuyingProductEventId, eventArgs);
        }

        #region ISupportAnalyticsStrategy

        void ISupportAnalyticsStrategy.OpenSupportWindowEvent()
        {
            OpenSupportWindowEvent();
        }

        void ISupportAnalyticsStrategy.StartBuyingProductEvent(string productId, string productName)
        {
            StartBuyingProductEvent(productId, productName);
        }

        #endregion
    }
}