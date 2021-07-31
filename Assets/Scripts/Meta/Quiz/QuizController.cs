using System.Collections.Generic;
using System.Linq;
using QuizFramework.Database;

namespace QuizFramework.Quiz
{
    public class QuizController : IQuizController
    {
        private readonly IQuestionDatabase _questionDatabase;

        private bool _hasNextQuestion;
        private IEnumerator<Question> _currentGroupQuestions;

        public QuizController(IQuestionDatabase questionDatabase)
        {
            _questionDatabase = questionDatabase;
        }

        private bool InitializeQuestionGroup(ushort group)
        {
            var questions = _questionDatabase.GetAllGroupQuestion(group);
            if (!questions.Any())
            {
                _hasNextQuestion = false;
                _currentGroupQuestions = null;
                return false;
            }

            _hasNextQuestion = true;
            _currentGroupQuestions = questions.GetEnumerator();
            _currentGroupQuestions.MoveNext();
            return true;
        }

        private Question? GetNextQuestion()
        {
            if (!_hasNextQuestion)
            {
                return null;
            }

            var currentQuestion = _currentGroupQuestions.Current;
            _hasNextQuestion = _currentGroupQuestions.MoveNext();
            return currentQuestion;
        }

        #region IQuizController

        bool IQuizController.HasNextQuestion => _hasNextQuestion;
        
        bool IQuizController.InitializeQuestionGroup(ushort group)
        {
            return InitializeQuestionGroup(group);
        }

        Question? IQuizController.GetNextQuestion()
        {
            return GetNextQuestion();
        }

        #endregion
    }
}