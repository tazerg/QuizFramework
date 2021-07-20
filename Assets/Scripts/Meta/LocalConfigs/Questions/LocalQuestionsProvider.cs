using System.IO;
using UnityEngine;

namespace QuizFramework.LocalConfig
{
    public class LocalQuestionsProvider : ILocalQuestionsProvider
    {
        private string GetLocalQuestions()
        {
            var questionsJsonAsset = Resources.Load<TextAsset>("QuestionDatabase");
            return questionsJsonAsset.text;
        }

        #region ILocalQuestionsProvider

        string ILocalQuestionsProvider.GetLocalQuestions()
        {
            return GetLocalQuestions();
        }

        #endregion
    }
}