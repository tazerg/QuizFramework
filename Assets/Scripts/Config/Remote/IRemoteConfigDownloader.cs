using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizFramework.RemoteConfig
{
    public interface IRemoteConfigDownloader
    {
        Task<List<string>> DownloadConfig(string configPath, string configId);
    }
}