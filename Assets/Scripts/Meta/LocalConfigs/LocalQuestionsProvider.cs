using System.IO;
using UnityEngine;

namespace QuizFramework.LocalConfig
{
    public class LocalQuestionsProvider : ILocalQuestionsProvider
    {
        public static readonly string QuestionsAssetPath = $"{Application.dataPath}/Resources/QuestionDatabase.json";
        
        private string GetLocalQuestions()
        {
            var localQuestionsJson = File.ReadAllText(QuestionsAssetPath);
            return localQuestionsJson;
        }

        #region ILocalQuestionsProvider

        string ILocalQuestionsProvider.GetLocalQuestions()
        {
            return GetLocalQuestions();
        }

        #endregion
    }
}