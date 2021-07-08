using UnityEngine;

namespace QuizFramework.LocalConfig
{
    [CreateAssetMenu(fileName = "LocalConfig", menuName = "Quiz/Local Config")]
    public class LocalConfig : ScriptableObject, ILocalConfig
    {
        [Header("Spreadsheet settings")]
        [SerializeField] private string _questionsSheetId;
        [SerializeField] private string _questionsTabId;
        [SerializeField] private string _versionControlTabId;

        [Header("Question settings")] 
        [SerializeField] private byte _answerStartIndex;
        [SerializeField] private byte _answerEndIndex;

        [Header("Advertisement")] 
        [SerializeField] private string _advertisementId;
        [SerializeField] private string _rewardedPlacementId;
        [SerializeField] private string _interstitialPlacementId;
        
        #region ILocalConfig

        string ILocalConfig.QuestionsSheetId => _questionsSheetId;
        string ILocalConfig.QuestionsTabId => _questionsTabId;
        string ILocalConfig.VersionControlTabId => _versionControlTabId;
        byte ILocalConfig.AnswersStartIndex => _answerStartIndex;
        byte ILocalConfig.AnswersEndIndex => _answerEndIndex;
        string ILocalConfig.AdvertisementId => _advertisementId;
        string ILocalConfig.RewardedPlacementId => _rewardedPlacementId;
        string ILocalConfig.InterstitialPlacementId => _interstitialPlacementId;

        #endregion
    }
}