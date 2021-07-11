using UnityEngine;

namespace QuizFramework.LocalConfig
{
    [CreateAssetMenu(fileName = "QuestionDownloaderConfig", menuName = "Quiz/Question Downloader Config")]
    public class QuestionDownloaderConfig : ScriptableObject, IQuestionDownloaderConfig
    {
        [Header("Spreadsheet settings")]
        [SerializeField] private string _questionsSheetId;
        [SerializeField] private string _questionsTabId;
        [SerializeField] private string _versionControlTabId;

        [Header("Question settings")] 
        [SerializeField] private byte _answerStartIndex;
        [SerializeField] private byte _answerEndIndex;
        
        #region IQuestionDownloaderConfig

        string IQuestionDownloaderConfig.QuestionsSheetId => _questionsSheetId;
        string IQuestionDownloaderConfig.QuestionsTabId => _questionsTabId;
        string IQuestionDownloaderConfig.VersionControlTabId => _versionControlTabId;
        byte IQuestionDownloaderConfig.AnswersStartIndex => _answerStartIndex;
        byte IQuestionDownloaderConfig.AnswersEndIndex => _answerEndIndex;

        #endregion
    }
}