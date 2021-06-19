using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QuizFramework.Config;
using QuizFramework.Storage;

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
        
        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ObjectCreationHandling = ObjectCreationHandling.Replace
        };

        private readonly ILocalStorage _localStorage;
        private readonly ILocalConfig _localConfig;
        private readonly IRemoteConfigDownloader _remoteConfigDownloader;
        
        public QuestionDatabaseLoader(ILocalStorage localStorage, ILocalConfig localConfig, 
            IRemoteConfigDownloader remoteConfigDownloader)
        {
            _localStorage = localStorage;
            _localConfig = localConfig;
            _remoteConfigDownloader = remoteConfigDownloader;
        }
        
        private IQuestionDatabase LoadFromLocal()
        {
            var localQuestions = _localStorage.GetLocalQuestions();
            var questionDatabase = JsonConvert.DeserializeObject<QuestionDatabase>(localQuestions, _jsonSettings);
            questionDatabase.FillQuestionGroupsDict();
            return questionDatabase;
        }

        private async Task<IQuestionDatabase> LoadFromRemote()
        {
            var questionsTabId = _localConfig.QuestionsTabId;
            var questionsTab = await _remoteConfigDownloader.DownloadConfig(questionsTabId);
            var questions = new List<Question>();
            for (var i = StartQuestionsRow; i < questionsTab.Count; i++)
            {
                var questionFields = questionsTab[i].Split(Separator);
                var question = new Question
                {
                    QuestionsGroup = ushort.Parse(questionFields[QuestionGroupValueIndex]),
                    QuestionStr = questionFields[QuestionValueIndex],
                    IndexOfCorrectAnswer = (byte)(byte.Parse(questionFields[CorrectAnswerValueIndex]) - NumberToIndexSubtrahend),
                    Answers = CollectAnswers(questionFields)
                };
                questions.Add(question);
            }
            
            var questionDatabase = new QuestionDatabase(questions);
            return questionDatabase;
        }

        private List<string> CollectAnswers(IReadOnlyList<string> questionFields)
        {
            var answers = new List<string>();
            for (var i = _localConfig.AnswersStartIndex; i <= _localConfig.AnswersEndIndex; i++)
            {
                answers.Add(questionFields[i]);
            }
            
            return answers;
        }

        #region IQuestionDatabaseLoader

        IQuestionDatabase IQuestionDatabaseLoader.LoadFromLocal()
        {
            return LoadFromLocal();
        }

        async Task<IQuestionDatabase> IQuestionDatabaseLoader.LoadFromRemote()
        {
            return await LoadFromRemote();
        }

        #endregion
    }
}