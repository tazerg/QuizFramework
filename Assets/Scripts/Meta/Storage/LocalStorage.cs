using Newtonsoft.Json;
using QuizFramework.Database;
using QuizFramework.Utils;
using UnityEngine;

namespace QuizFramework.Storage
{
    public class LocalStorage : ILocalStorage
    {
        private const string LocalVersionKey = "version";
        private const string LocalQuestionsKey = "questions";

        private const int DefaultLocalVersion = 0;
        private const string DefaultLocalQuestions = "";
        
        private int GetLocalVersion()
        {
            return PlayerPrefs.GetInt(LocalVersionKey, DefaultLocalVersion);
        }

        private string GetLocalQuestions()
        {
            return PlayerPrefs.GetString(LocalQuestionsKey, DefaultLocalQuestions);
        }

        private bool HasLocalStorage()
        {
            return PlayerPrefs.HasKey(LocalVersionKey);
        }

        private void SaveVersionToLocal(int version)
        {
            PlayerPrefs.SetInt(LocalVersionKey, version);
            PlayerPrefs.Save();
        }

        private void SaveQuestionToLocal(IQuestionDatabase questionDatabase)
        {
            var questionsJson = JsonConvert.SerializeObject(questionDatabase, JsonSettingsUtils.JsonSettings);
            PlayerPrefs.SetString(LocalQuestionsKey, questionsJson);
            PlayerPrefs.Save();
        }

        #region ILocalStorage

        int ILocalStorage.GetLocalVersion()
        {
            return GetLocalVersion();
        }

        string ILocalStorage.GetLocalQuestions()
        {
            return GetLocalQuestions();
        }

        bool ILocalStorage.HasLocalVersion()
        {
            return HasLocalStorage();
        }

        void ILocalStorage.SaveVersionToLocal(int version)
        {
            SaveVersionToLocal(version);
        }

        void ILocalStorage.SaveQuestionsToLocal(IQuestionDatabase questionDatabase)
        {
            SaveQuestionToLocal(questionDatabase);
        }

        #endregion
    }
}