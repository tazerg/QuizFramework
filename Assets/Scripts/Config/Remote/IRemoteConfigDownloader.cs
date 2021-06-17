using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizFramework.Config
{
    public interface IRemoteConfigDownloader
    {
        Task<List<string>> DownloadConfig(string tabId);
    }
}