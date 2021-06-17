using UnityEngine;

namespace QuizFramework.Config
{
    [CreateAssetMenu(fileName = "LocalConfig", menuName = "Quiz/Local Config")]
    public class LocalConfig : ScriptableObject, ILocalConfig
    {
        [SerializeField] private string _questionsSheetId;
        [SerializeField] private string _versionControlTabId;

        #region ILocalConfig

        string ILocalConfig.QuestionsSheetId => _questionsSheetId;
        string ILocalConfig.VersionControlTabId => _versionControlTabId;

        #endregion
    }
}