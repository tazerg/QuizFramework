using QuizFramework.Advertisement;
using UnityEngine;

namespace QuizFramework.LocalConfig
{
    [CreateAssetMenu(fileName = "AdsConfig", menuName = "Quiz/Ads Config")]
    public class AdsConfig : ScriptableObject, IAdsConfig
    {
        [SerializeField] private string _adProjectId;
        [SerializeField] private string _rewardedPlacementId;
        [SerializeField] private string _interstitialPlacementId;

        #region IAdsConfig
        
        string IAdsConfig.AdProjectId => _adProjectId;
        string IAdsConfig.RewardedPlacementId => _rewardedPlacementId;
        string IAdsConfig.InterstitialPlacementId => _interstitialPlacementId;

        #endregion
    }
}