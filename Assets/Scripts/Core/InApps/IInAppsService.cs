namespace QuizFramework.InApps
{
    public interface IInAppsService
    {
        bool IsInitialized { get; }
        void PurchaseProduct(string productId);
    }
}