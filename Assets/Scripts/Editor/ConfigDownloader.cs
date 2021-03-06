#if UNITY_EDITOR
using System.IO;
using Newtonsoft.Json;
using QuizFramework.Database;
using QuizFramework.LocalConfig;
using QuizFramework.RemoteConfig;
using QuizFramework.Utils;
using UnityEditor;
using UnityEngine;

namespace QuizFramework.Tools
{
    public class ConfigDownloader : MonoBehaviour
    {
        private static readonly string QuestionsAssetPath = $"{Application.dataPath}/Resources/QuestionDatabase.json";

        [MenuItem("QuizFramework/Download question database")]
        private static async void DownloadQuestionDatabase()
        {
            var localConfig = (IQuestionDownloaderConfig) Resources.Load<QuestionDownloaderConfig>("QuestionDownloaderConfig");
            var questionDatabaseLoader = (IQuestionDatabaseLoader) new QuestionDatabaseLoader(new SpreadsheetConfigDownloader());

            var questionDatabase = await questionDatabaseLoader.LoadFromRemote(localConfig.QuestionsSheetId,
                localConfig.QuestionsTabId, localConfig.AnswersStartIndex, localConfig.AnswersEndIndex);
            var questionDatabaseJson = JsonConvert.SerializeObject(questionDatabase, JsonSettingsUtils.JsonSettings);

            Directory.CreateDirectory(Path.GetDirectoryName(QuestionsAssetPath)!);
            File.WriteAllText(QuestionsAssetPath, questionDatabaseJson);
        }
    }
}
#endif