using System.Threading.Tasks;

namespace QuizFramework.Config
{
    public interface IVersionControlChecker
    {
        public Task<bool> IsCorrectVersion();
    }
}