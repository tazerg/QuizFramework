using QuizFramework.Database;

namespace QuizFramework.Storage
{
    public interface ILocalStorage
    {
        int GetLocalVersion();
        int GetMaxPassedLevel();
        string GetLocalQuestions();
        string GetPlayerId();
        bool HasLocalVersion();
        void SaveVersion(int version);
        void SaveMaxPassedLevel(int level);
        void SaveQuestions(IQuestionDatabase questionDatabase);
    }
}