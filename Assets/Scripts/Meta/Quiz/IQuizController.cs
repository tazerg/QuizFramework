using QuizFramework.Database;

namespace QuizFramework.Quiz
{
    public interface IQuizController
    {
        int QuestionsCount { get; }
        bool HasNextQuestion { get; }
        bool InitializeQuestionGroup(ushort group);
        Question? GetNextQuestion();
    }
}