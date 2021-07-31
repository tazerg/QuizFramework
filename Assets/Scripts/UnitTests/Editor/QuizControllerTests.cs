using System.Collections.Generic;
using NUnit.Framework;
using QuizFramework.Database;
using QuizFramework.Quiz;

namespace QuizFramework.UnitTests
{
    public class QuizControllerTests
    {
        private const ushort QuestionGroup = 1;

        private const string FirstQuestion = "first question";
        private const string SecondQuestion = "second question";
        private const string ThirdQuestion = "third question";
        
        private IQuizController _quizController;

        private readonly IEnumerable<Question> _questions = new List<Question>
        {
            new Question{ QuestionsGroup = QuestionGroup, QuestionStr = FirstQuestion},
            new Question{ QuestionsGroup = QuestionGroup, QuestionStr = SecondQuestion},
            new Question{ QuestionsGroup = QuestionGroup, QuestionStr = ThirdQuestion},
        };

        [SetUp]
        public void OnSetup()
        {
            var questionDatabase = new QuestionDatabase(_questions);
            _quizController = new QuizController(questionDatabase);
        }

        [TearDown]
        public void OnTearDown()
        {
            _quizController = null;
        }

        [Test]
        public void TestInitializeQuestionGroup()
        {
            var actual = _quizController.InitializeQuestionGroup(QuestionGroup);
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void TestGetNextQuestion()
        {
            _quizController.InitializeQuestionGroup(QuestionGroup);

            Assert.AreEqual(true, _quizController.HasNextQuestion);
            
            var currentQuestion = _quizController.GetNextQuestion();
            
            Assert.IsNotNull(currentQuestion);
            Assert.AreEqual(FirstQuestion, currentQuestion.Value.QuestionStr);
            Assert.AreEqual(true, _quizController.HasNextQuestion);

            currentQuestion = _quizController.GetNextQuestion();
            
            Assert.IsNotNull(currentQuestion);
            Assert.AreEqual(SecondQuestion, currentQuestion.Value.QuestionStr);
            Assert.AreEqual(true, _quizController.HasNextQuestion);
            
            currentQuestion = _quizController.GetNextQuestion();
            
            Assert.IsNotNull(currentQuestion);
            Assert.AreEqual(ThirdQuestion, currentQuestion.Value.QuestionStr);
            Assert.AreEqual(false, _quizController.HasNextQuestion);
            
            currentQuestion = _quizController.GetNextQuestion();
            
            Assert.IsNull(currentQuestion);
        }
    }
}