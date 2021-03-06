using System.Threading.Tasks;

namespace QuizFramework.Database
{
    public interface IQuestionDatabaseLoader
    {
        Task<IQuestionDatabase> LoadFromLocal(string questionsJson);
        Task<IQuestionDatabase> LoadFromRemote(string configPath, string questionsConfigId, int answersStartIndex, int answersEndIndex);
    }
}