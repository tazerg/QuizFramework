using QuizFramework.Advertisement;
using QuizFramework.EmailSenderToSelf;
using QuizFramework.LocalConfig;
using Zenject;

namespace QuizFramework.Installers
{
    public class MainSceneInstaller : MonoInstaller<ProjectRootInstaller>
    {
        public override void InstallBindings()
        {
            BindLocalConfigs();

            Container.BindInterfacesTo<UnityAdsService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<EmailSenderToSelf.EmailSenderToSelf>().AsSingle();
        }

        private void BindLocalConfigs()
        {
            Container.Bind<IAdsConfig>().To<AdsConfig>().FromScriptableObjectResource("AdsConfig").AsSingle();
            Container.Bind<ISocialNetworkConfig>().To<SocialNetworkConfig>().FromScriptableObjectResource("SocialNetworkConfig").AsSingle();
            Container.Bind<IEmailSenderToSelfConfig>().To<EmailSenderToSelfConfig>().FromScriptableObjectResource("EmailSenderToSelfConfig").AsSingle();
        }
    }
}