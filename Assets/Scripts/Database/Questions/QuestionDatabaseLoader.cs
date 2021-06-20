using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QuizFramework.RemoteConfig;
using QuizFramework.Utils;

namespace QuizFramework.Database
{
    public class QuestionDatabaseLoader : IQuestionDatabaseLoader
    {
        private const char Separator = ',';

        private const int NumberToIndexSubtrahend = 1;
        private const int StartQuestionsRow = 1;
        private const byte QuestionGroupValueIndex = 0;
        private const byte QuestionValueIndex = 1;
        private const byte CorrectAnswerValueIndex = 2;
        
        private readonly IRemoteConfigDownloader _remoteConfigDownloader;
        
        public QuestionDatabaseLoader(IRemoteConfigDownloader remoteConfigDownloader)
        {
            _remoteConfigDownloader = remoteConfigDownloader;
        }
        
        private IQuestionDatabase LoadFromLocal(string questionsJson)
        {
            var questionDatabase = JsonConvert.DeserializeObject<QuestionDatabase>(questionsJson, JsonSettingsUtils.JsonSettings);
            questionDatabase.FillQuestionGroupsDict();
            return questionDatabase;
        }

        private async Task<IQuestionDatabase> LoadFromRemote(string configPath, string questionsConfigId, int answersStartIndex, int answersEndIndex)
        {
            var questionsTab = await _remoteConfigDownloader.DownloadConfig(configPath, questionsConfigId);
            var questions = new List<Question>();
            for (var i = StartQuestionsRow; i < questionsTab.Count; i++)
            {
                var questionFields = questionsTab[i].Split(Separator);
                var question = new Question
                {
                    QuestionsGroup = ushort.Parse(questionFields[QuestionGroupValueIndex]),
                    QuestionStr = questionFields[QuestionValueIndex],
                    IndexOfCorrectAnswer = (byte)(byte.Parse(questionFields[CorrectAnswerValueIndex]) - NumberToIndexSubtrahend),
                    Answers = CollectAnswers(questionFields, answersStartIndex, answersEndIndex)
                };
                questions.Add(question);
            }
            
            var questionDatabase = new QuestionDatabase(questions);
            return questionDatabase;
        }

        private List<string> CollectAnswers(IReadOnlyList<string> questionFields, int answersStartIndex, int answersEndIndex)
        {
            var answers = new List<string>();
            for (var i = answersStartIndex; i <= answersEndIndex; i++)
            {
                answers.Add(questionFields[i]);
            }
            
            return answers;
        }

        #region IQuestionDatabaseLoader

        IQuestionDatabase IQuestionDatabaseLoader.LoadFromLocal(string questionsJson)
        {
            return LoadFromLocal(questionsJson);
        }

        async Task<IQuestionDatabase> IQuestionDatabaseLoader.LoadFromRemote(string configPath, string questionsConfigId, int answersStartIndex, int answersEndIndex)
        {
            return await LoadFromRemote(configPath, questionsConfigId, answersStartIndex, answersEndIndex);
        }

        #endregion
    }
}