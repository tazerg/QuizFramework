using System.Linq;

namespace QuizFramework.Database
{
    public class QuestionsFacade : IQuestionsFacade
    {
        private readonly IQuestionDatabase _questionDatabase;

        public QuestionsFacade(IQuestionDatabase questionDatabase)
        {
            _questionDatabase = questionDatabase;
        }

        bool HasNextGroup(ushort currentGroup)
        {
            var nextGroup = (ushort) (currentGroup + 1);
            var questions = _questionDatabase.GetAllGroupQuestion(nextGroup);
            return questions.Any();
        }

        #region IQuestionsFacade

        bool IQuestionsFacade.HasNextGroup(ushort currentGroup)
        {
            return HasNextGroup(currentGroup);
        }

        #endregion
    }
}