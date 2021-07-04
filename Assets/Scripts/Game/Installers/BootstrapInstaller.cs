using QuizFramework.Database;
using QuizFramework.LocalConfig;
using QuizFramework.RemoteConfig;
using QuizFramework.Storage;
using QuizFramework.UI;
using QuizFramework.VersionControl;
using Zenject;

namespace QuizFramework.Installers
{
    public class BootstrapInstaller : MonoInstaller<BootstrapInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ILocalConfig>().To<LocalConfig.LocalConfig>().FromScriptableObjectResource("LocalConfig").AsSingle();
            
            Container.Bind<IRemoteConfigDownloader>().To<SpreadsheetConfigDownloader>().AsSingle();
            Container.Bind<IQuestionDatabaseLoader>().To<QuestionDatabaseLoader>().AsSingle();
            Container.Bind<ILocalQuestionsProvider>().To<LocalQuestionsProvider>().AsSingle();
            Container.Bind<IVersionChecker>().To<VersionChecker>().AsSingle();
            Container.Bind<ILocalStorage>().To<LocalStorage>().AsSingle();

            var loadingVM = FindObjectOfType<LoadingVM>();
            Container.Bind<LoadingVM>().FromInstance(loadingVM).AsSingle();
        }
    }
}