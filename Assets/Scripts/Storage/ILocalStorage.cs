namespace QuizFramework.Storage
{
    public interface ILocalStorage
    {
        int GetLocalVersion();
        string GetLocalQuestions();
        void SaveToLocalVersion();
        void SaveToLocalQuestions();
    }
}