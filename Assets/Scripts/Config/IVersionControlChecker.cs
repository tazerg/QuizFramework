using System.Threading.Tasks;

namespace QuizFramework.Config
{
    public interface IVersionControlChecker
    {
        public int? RemoteVersion { get; }
        public Task<bool> IsCorrectVersion();
    }
}