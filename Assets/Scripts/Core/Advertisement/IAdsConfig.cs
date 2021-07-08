namespace QuizFramework.Advertisement
{
    public interface IAdsConfig
    {
        string AdProjectId { get; }
        string RewardedPlacementId { get; }
        string InterstitialPlacementId { get; }
    }
}