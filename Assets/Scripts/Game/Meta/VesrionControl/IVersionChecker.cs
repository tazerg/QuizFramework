using System.Threading.Tasks;

namespace QuizFramework.VersionControl
{
    public interface IVersionChecker
    {
        public int? RemoteVersion { get; }
        public Task<bool> IsCorrectVersion(string configPath, string versionConfigId);
    }
}