using QuizFramework.Advertisement;
using QuizFramework.LocalConfig;
using Zenject;

namespace QuizFramework.Installers
{
    public class MainSceneInstaller : MonoInstaller<ProjectRootInstaller>
    {
        public override void InstallBindings()
        {
            BindLocalConfigs();

            Container.Bind<IAdsService>().To<UnityAdsService>().AsSingle().NonLazy();
        }

        private void BindLocalConfigs()
        {
            Container.Bind<IAdsConfig>().To<AdsConfig>().FromScriptableObjectResource("AdsConfig").AsSingle();
            Container.Bind<ISocialNetworkConfig>().To<SocialNetworkConfig>().FromScriptableObjectResource("SocialNetworkConfig").AsSingle();
        }
    }
}