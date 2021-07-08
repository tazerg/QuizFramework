using QuizFramework.Advertisement;
using QuizFramework.LocalConfig;
using Zenject;

namespace QuizFramework.Installers
{
    public class MainSceneInstaller : MonoInstaller<ProjectRootInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IAdsConfig>().To<AdsConfig>().FromScriptableObjectResource("AdsConfig").AsSingle();
            
            Container.Bind<IAdsService>().To<UnityAdsService>().AsSingle().NonLazy();
        }
    }
}