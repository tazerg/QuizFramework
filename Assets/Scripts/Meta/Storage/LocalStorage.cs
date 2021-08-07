using System;
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
        private const string PlayerIdKey = "playerId";
        private const string MaxPassedLevelKey = "maxPassedLevel";

        private const int IncorrectValue = -1;
        private const int DefaultValue = 0;
        private const string DefaultLocalQuestions = "";

        private int _cachedVersionNumber = IncorrectValue;
        private int _cachedMaxPassedLevel = IncorrectValue;
        private string _cachedPlayerId;
        
        private int GetLocalVersion()
        {
            TrySetCachedIntValueFromPrefs(LocalVersionKey, ref _cachedVersionNumber);
            return _cachedVersionNumber;
        }

        private int GetMaxPassedLevel()
        {
            TrySetCachedIntValueFromPrefs(MaxPassedLevelKey, ref _cachedMaxPassedLevel);
            return _cachedMaxPassedLevel;
        }

        private string GetLocalQuestions()
        {
            return PlayerPrefs.GetString(LocalQuestionsKey, DefaultLocalQuestions);
        }

        private string GetPlayerId()
        {
            if (!string.IsNullOrEmpty(_cachedPlayerId))
            {
                return _cachedPlayerId;
            }
            
            _cachedPlayerId = PlayerPrefs.GetString(PlayerIdKey);
            if (!string.IsNullOrEmpty(_cachedPlayerId))
            {
                return _cachedPlayerId;
            }

            _cachedPlayerId = GeneratePlayerId();
            SaveCachedValueToPrefs(PlayerIdKey, _cachedPlayerId);
            PlayerPrefs.Save();
            return _cachedPlayerId;
        }

        private bool HasLocalStorage()
        {
            return PlayerPrefs.HasKey(LocalVersionKey);
        }

        private void SaveVersion(int version)
        {
            _cachedVersionNumber = version;
            SaveCachedValueToPrefs(LocalVersionKey, _cachedVersionNumber);
        }
        
        private void SaveMaxPassedLevel(int level)
        {
            _cachedMaxPassedLevel = level;
            SaveCachedValueToPrefs(MaxPassedLevelKey, _cachedMaxPassedLevel);
        }

        private void SaveQuestion(IQuestionDatabase questionDatabase)
        {
            var questionsJson = JsonConvert.SerializeObject(questionDatabase, JsonSettingsUtils.JsonSettings);
            SaveCachedValueToPrefs(LocalQuestionsKey, questionsJson);
        }

        private void TrySetCachedIntValueFromPrefs(string prefKey, ref int cachedValue)
        {
            if (cachedValue != IncorrectValue)
            {
                return;
            }

            cachedValue = PlayerPrefs.GetInt(prefKey, DefaultValue);
        }

        private void SaveCachedValueToPrefs(string prefKey, int cachedValue)
        {
            PlayerPrefs.SetInt(prefKey, cachedValue);
            PlayerPrefs.Save();
        }
        
        private void SaveCachedValueToPrefs(string prefKey, string cachedValue)
        {
            PlayerPrefs.SetString(prefKey, cachedValue);
            PlayerPrefs.Save();
        }

        private string GeneratePlayerId()
        {
            return Guid.NewGuid().ToString();
        }

        #region ILocalStorage

        int ILocalStorage.GetLocalVersion()
        {
            return GetLocalVersion();
        }

        int ILocalStorage.GetMaxPassedLevel()
        {
            return GetMaxPassedLevel();
        }

        string ILocalStorage.GetLocalQuestions()
        {
            return GetLocalQuestions();
        }

        string ILocalStorage.GetPlayerId()
        {
            return GetPlayerId();
        }

        bool ILocalStorage.HasLocalVersion()
        {
            return HasLocalStorage();
        }

        void ILocalStorage.SaveVersion(int version)
        {
            SaveVersion(version);
        }

        void ILocalStorage.SaveMaxPassedLevel(int level)
        {
            SaveMaxPassedLevel(level);
        }

        void ILocalStorage.SaveQuestions(IQuestionDatabase questionDatabase)
        {
            SaveQuestion(questionDatabase);
        }

        #endregion
    }
}