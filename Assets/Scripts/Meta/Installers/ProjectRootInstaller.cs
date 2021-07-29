using QuizFramework.SignalBus;
using QuizFramework.Storage;
using Zenject;

namespace QuizFramework.Installers
{
    public class ProjectRootInstaller : MonoInstaller<ProjectRootInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ISignalBus>().To<SignalBus.SignalBus>().AsSingle();
            Container.Bind<ILocalStorage>().To<LocalStorage>().AsSingle();
        }
    }
}