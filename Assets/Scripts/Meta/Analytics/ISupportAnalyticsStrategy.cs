namespace QuizFramework.Analytics
{
    public interface ISupportAnalyticsStrategy
    {
        void OpenSupportWindowEvent();
        void StartBuyingProductEvent(string productId, string productName);
    }
}