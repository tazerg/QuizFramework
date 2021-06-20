using QuizFramework.Database;

namespace QuizFramework.Storage
{
    public interface ILocalStorage
    {
        int GetLocalVersion();
        string GetLocalQuestions();
        void SaveVersionToLocal(int version);
        void SaveQuestionsToLocal(IQuestionDatabase questionDatabase);
    }
}