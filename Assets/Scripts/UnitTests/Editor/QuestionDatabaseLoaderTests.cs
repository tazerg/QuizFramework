using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QuizFramework.Database;
using QuizFramework.RemoteConfig;
using QuizFramework.Storage;
using UnityEngine.TestTools;
using Zenject;

namespace QuizFramework.UnitTests
{
    public class QuestionDatabaseLoaderTests
    {
        private const ushort FirstGroup = 1;
        private const int FirstGroupQuestionsCount = 2;
        private const ushort SecondGroup = 2;
        private const int SecondGroupQuestionsCount = 1;
        private const ushort ThirdGroup = 3;
        private const int ThirdGroupQuestionsCount = 0;

        private const ushort FirsQuestionGroup = 1;
        private const string FirstQuestionStr = "Вопрос1";
        private const byte FirstQuestionCorrectAnswerIndex = 1;
        
        private const ushort SecondQuestionGroup = 1;
        private const string SecondQuestionStr = "Вопрос2";
        private const byte SecondQuestionCorrectAnswerIndex = 0;
        
        private const ushort ThirdQuestionGroup = 2;
        private const string ThirdQuestionStr = "Вопрос3";
        private const byte ThirdQuestionCorrectAnswerIndex = 2;
        
        private const int AnswersCount = 3;
        private const string FirstAnswerStr = "Ответ1";
        private const string SecondAnswerStr = "Ответ2";
        private const string ThirdAnswerStr = "Ответ3";

        private const byte AnswerStartIndex = 3;
        private const byte AnswerEndIndex = 5;
        
        private const string RealConfigSpreadsheetId = "1VLeAvWe5BWLM81129hkqF5pdZTwatAyVj1pHxZVYNOE";
        private const string RealQuestionTabId = "1215663197";

        private readonly string _localQuestionDatabaseJson = new StringBuilder()
            .Append("{\n")
            .Append("\t\"questions\":\n")
            .Append("\t[\n")
            .Append("\t\t{\n")
            .Append("\t\t\t\"QuestionsGroup\":1,\n")
            .Append("\t\t\t\"QuestionStr\":\"Вопрос1\",\n")
            .Append("\t\t\t\"Answers\":\n")
            .Append("\t\t\t[\n")
            .Append("\t\t\t\t\"Ответ1\",\n")
            .Append("\t\t\t\t\"Ответ2\",\n")
            .Append("\t\t\t\t\"Ответ3\"\n")
            .Append("\t\t\t],\n")
            .Append("\t\t\t\"IndexOfCorrectAnswer\":1\n")
            .Append("\t\t},\n")
            .Append("\t\t{\n")
            .Append("\t\t\t\"QuestionsGroup\":1,\n")
            .Append("\t\t\t\"QuestionStr\":\"Вопрос2\",\n")
            .Append("\t\t\t\"Answers\":\n")
            .Append("\t\t\t[\n")
            .Append("\t\t\t\t\"Ответ1\",\n")
            .Append("\t\t\t\t\"Ответ2\",\n")
            .Append("\t\t\t\t\"Ответ3\"\n")
            .Append("\t\t\t],\n")
            .Append("\t\t\t\"IndexOfCorrectAnswer\":0\n")
            .Append("\t\t},\n")
            .Append("\t\t{\n")
            .Append("\t\t\t\"QuestionsGroup\":2,\n")
            .Append("\t\t\t\"QuestionStr\":\"Вопрос3\",\n")
            .Append("\t\t\t\"Answers\":\n")
            .Append("\t\t\t[\n")
            .Append("\t\t\t\t\"Ответ1\",\n")
            .Append("\t\t\t\t\"Ответ2\",\n")
            .Append("\t\t\t\t\"Ответ3\"\n")
            .Append("\t\t\t],\n")
            .Append("\t\t\t\"IndexOfCorrectAnswer\":2\n")
            .Append("\t\t}\n")
            .Append("\t]\n")
            .Append("}")
            .ToString();
        
        private readonly List<string> _mockedRemoteQuestionDatabase = new List<string>
        {
            "Group,Question,Number,Answer1,Answer2,Answer2",
            "1,Вопрос1,2,Ответ1,Ответ2,Ответ3",
            "1,Вопрос2,1,Ответ1,Ответ2,Ответ3",
            "2,Вопрос3,3,Ответ1,Ответ2,Ответ3"
        };

        private Mock<ILocalStorage> _mockLocalStorage;
        private DiContainer _diContainer;
        private IQuestionDatabaseLoader _questionDatabaseLoader;
        
        [SetUp]
        public void OnSetup()
        {
            _diContainer = new DiContainer(StaticContext.Container);
            
            _mockLocalStorage = new Mock<ILocalStorage>();
            
            _diContainer.BindInstance(_mockLocalStorage.Object).AsSingle();
            _diContainer.Bind<IQuestionDatabaseLoader>().To<QuestionDatabaseLoader>().AsSingle();
        }

        [TearDown]
        public void OnTearDown()
        {
            StaticContext.Clear();

            _diContainer = null;
            _mockLocalStorage = null;
            _questionDatabaseLoader = null;
        }

        [Test]
        public void TestLoadFromLocal()
        {
            var mockRemoteConfigDownloader = new Mock<IRemoteConfigDownloader>();
            _diContainer.BindInstance(mockRemoteConfigDownloader.Object).AsSingle();
            _mockLocalStorage.Setup(x => x.GetLocalQuestions()).Returns(_localQuestionDatabaseJson);

            _questionDatabaseLoader = _diContainer.Resolve<IQuestionDatabaseLoader>();

            var actual = Task
                .Run(async () => await _questionDatabaseLoader.LoadFromLocal(_localQuestionDatabaseJson))
                .GetAwaiter().GetResult();
            Assert.NotNull(actual);

            var actualLoadCorrected = IsDatabaseLoadCorrected(actual);
            Assert.AreEqual(true, actualLoadCorrected);
        }

        [Test]
        public void TestMockedLoadFromRemote()
        {
            var mockRemoteConfigDownloader = new Mock<IRemoteConfigDownloader>();
            _diContainer.BindInstance(mockRemoteConfigDownloader.Object).AsSingle();

            mockRemoteConfigDownloader.Setup(x => x.DownloadConfig(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_mockedRemoteQuestionDatabase));
            
            _questionDatabaseLoader = _diContainer.Resolve<IQuestionDatabaseLoader>();

            var actual = Task
                .Run(async () =>
                    await _questionDatabaseLoader.LoadFromRemote(string.Empty, string.Empty, AnswerStartIndex,
                        AnswerEndIndex)).GetAwaiter().GetResult();
            Assert.NotNull(actual);

            var actualLoadCorrected = IsDatabaseLoadCorrected(actual);
            Assert.AreEqual(true, actualLoadCorrected);
        }
        
        //Use UnityTest attribute because UnityWebRequest should use in main thread and current NUnit framework
        //don't support async await methods
        [UnityTest]
        public IEnumerator TestRealLoadFromRemote()
        {
            _diContainer.Bind<IRemoteConfigDownloader>().To<SpreadsheetConfigDownloader>().AsSingle();
            
            _questionDatabaseLoader = _diContainer.Resolve<IQuestionDatabaseLoader>();

            var task = _questionDatabaseLoader.LoadFromRemote(RealConfigSpreadsheetId, RealQuestionTabId,
                AnswerStartIndex, AnswerEndIndex);
            while (!task.IsCompleted)
            {
                yield return null;
            }

            var actual = task.Result;
            Assert.NotNull(actual);

            var actualLoadCorrected = IsDatabaseLoadCorrected(actual);
            Assert.AreEqual(true, actualLoadCorrected);
        }

        private bool IsDatabaseLoadCorrected(IQuestionDatabase questionDatabase)
        {
            var firstGroupQuestions = questionDatabase.GetAllGroupQuestion(FirstGroup).ToList();
            if (FirstGroupQuestionsCount != firstGroupQuestions.Count)
                return false;

            var firstQuestion = firstGroupQuestions[0];
            var actualFirstQuestionLoadCorrect = IsQuestionLoadCorrected(firstQuestion, FirsQuestionGroup, FirstQuestionStr,
                FirstQuestionCorrectAnswerIndex, AnswersCount, FirstAnswerStr, SecondAnswerStr, ThirdAnswerStr);
            if (!actualFirstQuestionLoadCorrect)
                return false;
            
            var secondQuestion = firstGroupQuestions[1];
            var actualSecondQuestionLoadCorrect = IsQuestionLoadCorrected(secondQuestion, SecondQuestionGroup,
                SecondQuestionStr, SecondQuestionCorrectAnswerIndex, AnswersCount, FirstAnswerStr, SecondAnswerStr,
                ThirdAnswerStr);
            if (!actualSecondQuestionLoadCorrect)
                return false;

            var secondGroupQuestions = questionDatabase.GetAllGroupQuestion(SecondGroup).ToList();
            if (SecondGroupQuestionsCount != secondGroupQuestions.Count)
                return false;

            var thirdQuestion = secondGroupQuestions[0];
            var actualThirdQuestionLoadCorrect = IsQuestionLoadCorrected(thirdQuestion, ThirdQuestionGroup, ThirdQuestionStr,
                ThirdQuestionCorrectAnswerIndex, AnswersCount, FirstAnswerStr, SecondAnswerStr, ThirdAnswerStr);
            if (!actualThirdQuestionLoadCorrect)
                return false;

            var thirdGroupQuestionsCount = questionDatabase.GetAllGroupQuestion(ThirdGroup).Count();
            return ThirdGroupQuestionsCount == thirdGroupQuestionsCount;
        }

        private bool IsQuestionLoadCorrected(Question actual, ushort expectedGroup, string expectedQuestion, 
            byte expectedCorrectAnswerIndex, int expectedAnswersCount, params string[] expectedAnswers)
        {
            var actualGroup = actual.QuestionsGroup;
            if (expectedGroup != actualGroup)
                return false;

            var actualQuestion = actual.QuestionStr;
            if (expectedQuestion != actualQuestion)
                return false;

            var actualCorrectAnswerIndex = actual.IndexOfCorrectAnswer;
            if (expectedCorrectAnswerIndex != actualCorrectAnswerIndex)
                return false;

            var actualAnswers = actual.Answers;
            if (expectedAnswersCount != actualAnswers.Count)
                return false;

            for (var i = 0; i < actualAnswers.Count; i++)
            {
                if (actualAnswers[i] != expectedAnswers[i])
                    return false;
            }

            return true;
        }
    }
}