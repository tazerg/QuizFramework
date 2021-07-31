using QuizFramework.Database;

namespace QuizFramework.Quiz
{
    public interface IQuizController
    {
        bool HasNextQuestion { get; }
        bool InitializeQuestionGroup(ushort group);
        Question? GetNextQuestion();
    }
}