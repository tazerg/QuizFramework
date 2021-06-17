using System.Threading.Tasks;
using QuizFramework.Storage;
using UnityEngine;

namespace QuizFramework.Config
{
    public class VersionControlChecker : IVersionControlChecker
    {
        private const char Separator = ',';
        private const int VersionTabValueIndex = 1;
        
        private readonly ILocalConfig _localConfig;
        private readonly ILocalStorage _localStorage;
        private readonly IRemoteConfigDownloader _remoteConfigDownloader;
        
        public VersionControlChecker(ILocalConfig localConfig, ILocalStorage localStorage, IRemoteConfigDownloader remoteConfigDownloader)
        {
            _localConfig = localConfig;
            _localStorage = localStorage;
            _remoteConfigDownloader = remoteConfigDownloader;
        }
        
        private async Task<bool> IsCorrectVersion()
        {
            var versionTab = _localConfig.VersionControlTabId;
            var remoteVersionTab = await _remoteConfigDownloader.DownloadConfig(versionTab);
            var remoteVersionArray = remoteVersionTab[0].Split(Separator);
            if (!int.TryParse(remoteVersionArray[VersionTabValueIndex], out var remoteVersion))
            {
                Debug.LogError("Can't parse remote version");
                return false;
            }

            var localVersion = _localStorage.GetLocalVersion();
            return localVersion == remoteVersion;
        }

        #region IVersionControlChecker

        async Task<bool> IVersionControlChecker.IsCorrectVersion()
        {
            return await IsCorrectVersion();
        }

        #endregion
    }
}