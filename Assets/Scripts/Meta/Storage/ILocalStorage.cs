using QuizFramework.Database;

namespace QuizFramework.Storage
{
    public interface ILocalStorage
    {
        int GetLocalVersion();
        string GetLocalQuestions();
        bool HasLocalVersion();
        void SaveVersionToLocal(int version);
        void SaveQuestionsToLocal(IQuestionDatabase questionDatabase);
    }
}