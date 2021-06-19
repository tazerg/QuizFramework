using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Zenject;
using NUnit.Framework;
using QuizFramework.Config;
using QuizFramework.Storage;
using UnityEngine.TestTools;

namespace QuizFramework.UnitTests
{
    public class VersionControlCheckerTests
    {
        private const int NonInstalledVersion = 0;
        private const bool NonInstalledVersionCheckResult = false;
        private const int FirstInstalledVersion = 1;
        private const bool FirstInstalledVersionCheckResult = true;

        private const string RealConfigSpreadsheetId = "1VLeAvWe5BWLM81129hkqF5pdZTwatAyVj1pHxZVYNOE";
        private const string RealVersionTabId = "0";
        private const int RealRemoteVersion = 1;

        private readonly List<string> _mockedRemoteVersionConfig = new List<string>
        {
            "Version,1"
        };
        
        private Mock<ILocalConfig> _mockLocalConfig;
        private Mock<ILocalStorage> _mockLocalStorage;
        private DiContainer _diContainer;
        private IVersionControlChecker _versionControlChecker;
        
        [SetUp]
        public void OnSetup()
        {
            _diContainer = new DiContainer(StaticContext.Container);

            _mockLocalConfig = new Mock<ILocalConfig>();
            _mockLocalStorage = new Mock<ILocalStorage>();

            _diContainer.BindInstance(_mockLocalConfig.Object).AsSingle();
            _diContainer.BindInstance(_mockLocalStorage.Object).AsSingle();
            _diContainer.Bind<IVersionControlChecker>().To<VersionControlChecker>().AsSingle();
        }

        [TearDown]
        public void OnTeardown()
        {
            StaticContext.Clear();
            
            _versionControlChecker = null;
            _mockLocalConfig = null;
            _mockLocalStorage = null;
            _diContainer = null;
        }

        [TestCase(NonInstalledVersion, NonInstalledVersionCheckResult)]
        [TestCase(FirstInstalledVersion, FirstInstalledVersionCheckResult)]
        public void TestMoqVersionChecker(int installedVersion, bool expected)
        {
            var mockRemoteConfigDownloader = new Mock<IRemoteConfigDownloader>();
            _diContainer.Bind<IRemoteConfigDownloader>().FromInstance(mockRemoteConfigDownloader.Object).AsSingle();

            _mockLocalConfig.SetupGet(x => x.VersionControlTabId).Returns(string.Empty);
            _mockLocalStorage.Setup(x => x.GetLocalVersion()).Returns(installedVersion);
            mockRemoteConfigDownloader.Setup(x => x.DownloadConfig(It.IsAny<string>()))
                                        .Returns(Task.FromResult(_mockedRemoteVersionConfig));

            _versionControlChecker = _diContainer.Resolve<IVersionControlChecker>();
            
            var actual = Task.Run(async () => await IsCorrectVersion()).GetAwaiter().GetResult();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestGetRemoteVersion()
        {
            var mockRemoteConfigDownloader = new Mock<IRemoteConfigDownloader>();
            _diContainer.Bind<IRemoteConfigDownloader>().FromInstance(mockRemoteConfigDownloader.Object).AsSingle();

            _mockLocalConfig.SetupGet(x => x.VersionControlTabId).Returns(string.Empty);
            _mockLocalStorage.Setup(x => x.GetLocalVersion()).Returns(It.IsAny<int>());
            mockRemoteConfigDownloader.Setup(x => x.DownloadConfig(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockedRemoteVersionConfig));

            _versionControlChecker = _diContainer.Resolve<IVersionControlChecker>();
            
            Assert.IsNull(_versionControlChecker.RemoteVersion);
            Task.Run(async () => await IsCorrectVersion()).GetAwaiter().GetResult();

            var actual = _versionControlChecker.RemoteVersion;
            Assert.IsNotNull(actual);
            Assert.AreEqual(RealRemoteVersion, actual.Value);
        }

        //Use UnityTest attribute because UnityWebRequest should use in main thread and current NUnit framework
        //don't support async await methods
        [UnityTest]
        public IEnumerator TestRealVersionIsCorrected()
        {
            BindAndResolveInRealTests(FirstInstalledVersion);

            var task = IsCorrectVersion();
            while (!task.IsCompleted)
            {
                yield return null;
            }
            
            Assert.AreEqual(FirstInstalledVersionCheckResult, task.Result);
        }
        
        [UnityTest]
        public IEnumerator TestRealVersionIsWrong()
        {
            BindAndResolveInRealTests(NonInstalledVersion);

            var task = IsCorrectVersion();
            while (!task.IsCompleted)
            {
                yield return null;
            }
            
            Assert.AreEqual(NonInstalledVersionCheckResult, task.Result);
        }

        private void BindAndResolveInRealTests(int installedVersion)
        {
            _diContainer.Bind<IRemoteConfigDownloader>().To<SpreadsheetConfigDownloader>().AsSingle();

            _mockLocalConfig.SetupGet(x => x.QuestionsSheetId).Returns(RealConfigSpreadsheetId);
            _mockLocalConfig.SetupGet(x => x.VersionControlTabId).Returns(RealVersionTabId);
            _mockLocalStorage.Setup(x => x.GetLocalVersion()).Returns(installedVersion);
            
            _versionControlChecker = _diContainer.Resolve<IVersionControlChecker>();
        }

        private async Task<bool> IsCorrectVersion()
        {
            return await _versionControlChecker.IsCorrectVersion();
        }
    }
}