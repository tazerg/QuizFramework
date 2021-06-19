using System.Threading.Tasks;

namespace QuizFramework.Database
{
    public interface IQuestionDatabaseLoader
    {
        IQuestionDatabase LoadFromLocal();
        Task<IQuestionDatabase> LoadFromRemote();
    }
}