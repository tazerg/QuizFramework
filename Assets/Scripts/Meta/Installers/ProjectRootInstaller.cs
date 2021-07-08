using QuizFramework.SignalBus;
using Zenject;

namespace QuizFramework.Installers
{
    public class ProjectRootInstaller : MonoInstaller<ProjectRootInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ISignalBus>().To<SignalBus.SignalBus>().AsSingle();
        }
    }
}