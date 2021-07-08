using QuizFramework.Advertisement;
using UnityEngine.Advertisements;
using Zenject;

namespace QuizFramework.Installers
{
    public class MainSceneInstaller : MonoInstaller<ProjectRootInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IUnityAdsShowListener>().To<UnityAdsShowListener>().AsSingle();
            
            Container.Bind<IAdsService>().To<UnityAdsService>().AsSingle().NonLazy();
        }
    }
}