using System.Threading.Tasks;

namespace QuizFramework.Advertisement
{
    public interface IAdsService
    {
        bool IsReady(AdPlacement placement);
        Task<AdShowResult> ShowAd(AdPlacement placement);
    }
}