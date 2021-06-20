using System.Threading.Tasks;
using QuizFramework.RemoteConfig;
using QuizFramework.Storage;
using UnityEngine;

namespace QuizFramework.VersionControl
{
    public class VersionChecker : IVersionChecker
    {
        private const char Separator = ',';
        private const int VersionTabValueIndex = 1;
        
        private readonly ILocalStorage _localStorage;
        private readonly IRemoteConfigDownloader _remoteConfigDownloader;

        private int? _remoteVersion;
        
        public VersionChecker(ILocalStorage localStorage, IRemoteConfigDownloader remoteConfigDownloader)
        {
            _localStorage = localStorage;
            _remoteConfigDownloader = remoteConfigDownloader;
        }
        
        private async Task<bool> IsCorrectVersion(string configPath, string versionConfigId)
        {
            var remoteVersionTab = await _remoteConfigDownloader.DownloadConfig(configPath, versionConfigId);
            var remoteVersionArray = remoteVersionTab[0].Split(Separator);
            if (!int.TryParse(remoteVersionArray[VersionTabValueIndex], out var remoteVersion))
            {
                Debug.LogError("Can't parse remote version");
                return false;
            }

            _remoteVersion = remoteVersion;
            var localVersion = _localStorage.GetLocalVersion();
            return localVersion == _remoteVersion.Value;
        }

        #region IVersionChecker

        int? IVersionChecker.RemoteVersion => _remoteVersion;

        async Task<bool> IVersionChecker.IsCorrectVersion(string configPath, string versionConfigId)
        {
            return await IsCorrectVersion(configPath, versionConfigId);
        }

        #endregion
    }
}