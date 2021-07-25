using QuizFramework.Advertisement;
using QuizFramework.Core;
using QuizFramework.EmailSenderToSelf;
using QuizFramework.LocalConfig;
using QuizFramework.Notifications;
using UnityEngine;
using Zenject;

namespace QuizFramework.Installers
{
    public class MainSceneInstaller : MonoInstaller<ProjectRootInstaller>
    {
        public override void InstallBindings()
        {
            BindLocalConfigs();
            BindNotificationDatabase();

            Container.BindInterfacesTo<UnityAdsService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<EmailSenderToSelf.EmailSenderToSelf>().AsSingle();
            Container.BindInterfacesTo<NotificationController>().AsSingle();
            Container.BindInterfacesTo<NotificationsFactory>().AsSingle();
            Container.BindInterfacesTo<ClientTImeProvider>().AsSingle();

#if UNITY_EDITOR
            Container.BindInterfacesTo<DummyNotificationSender>().AsSingle();
#elif UNITY_ANDROID
            Container.BindInterfacesTo<AndroidNotificationSender>().AsSingle();
#endif
        }

        private void BindLocalConfigs()
        {
            Container.Bind<IAdsConfig>().To<AdsConfig>().FromScriptableObjectResource("AdsConfig").AsSingle();
            Container.Bind<ISocialNetworkConfig>().To<SocialNetworkConfig>().FromScriptableObjectResource("SocialNetworkConfig").AsSingle();
            Container.Bind<IEmailSenderToSelfConfig>().To<EmailSenderToSelfConfig>().FromScriptableObjectResource("EmailSenderToSelfConfig").AsSingle();
            Container.Bind<INotificationConfig>().To<NotificationConfig>().FromScriptableObjectResource("NotificationConfig").AsSingle();
        }

        private void BindNotificationDatabase()
        {
            var notificationsDatabaseAsset = Resources.Load<TextAsset>("NotificationsDatabase");
            if (notificationsDatabaseAsset == null)
            {
                Debug.LogError("Can't find notifications database asset!");
                return;
            }

            var notificationDatabase = NotificationDatabaseLoader.LoadDatabase(notificationsDatabaseAsset.text);
            Container.BindInstance<INotificationDatabase>(notificationDatabase).AsSingle();
        }
    }
}