using QuizFramework.Advertisement;
using QuizFramework.Analytics;
using QuizFramework.Core;
using QuizFramework.Database;
using QuizFramework.EmailSender;
using QuizFramework.InApps;
using QuizFramework.LocalConfig;
using QuizFramework.LocalConfigs;
using QuizFramework.Notifications;
using QuizFramework.Quiz;
using UnityEngine;
using Zenject;

namespace QuizFramework.Installers
{
    public class MainSceneInstaller : MonoInstaller<ProjectRootInstaller>
    {
        public override void InstallBindings()
        {
            BindLocalConfigs();
            BindFacades();
            BindNotifications();
            BindAnalytics();

            Container.BindInterfacesTo<UnityAdsService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<UnityInAppsService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<EmailSenderToSelf>().AsSingle();
            Container.BindInterfacesTo<ClientTimeProvider>().AsSingle();
            Container.BindInterfacesTo<QuizController>().AsSingle();
        }

        private void BindLocalConfigs()
        {
            Container.Bind<IAdsConfig>().To<AdsConfig>().FromScriptableObjectResource("AdsConfig").AsSingle();
            Container.Bind<ISocialNetworkConfig>().To<SocialNetworkConfig>().FromScriptableObjectResource("SocialNetworkConfig").AsSingle();
            Container.Bind<IEmailSenderToSelfConfig>().To<EmailSenderToSelfConfig>().FromScriptableObjectResource("EmailSenderToSelfConfig").AsSingle();
            Container.Bind<INotificationConfig>().To<NotificationConfig>().FromScriptableObjectResource("NotificationConfig").AsSingle();
            Container.Bind<IQuizResultConfig>().To<QuizResultConfig>().FromScriptableObjectResource("QuizResultConfig").AsSingle();
            Container.Bind<IInAppsConfig>().To<InAppsConfig>().FromScriptableObjectResource("InAppsConfig").AsSingle();
        }

        private void BindFacades()
        {
            Container.Bind<IQuestionsFacade>().To<QuestionsFacade>().AsSingle();
        }

        private void BindNotifications()
        {
            BindNotificationDatabase();
            
            Container.BindInterfacesTo<NotificationController>().AsSingle();
            Container.BindInterfacesTo<NotificationsFactory>().AsSingle();
#if UNITY_EDITOR
            Container.BindInterfacesTo<DummyNotificationSender>().AsSingle();
#elif UNITY_ANDROID
            Container.BindInterfacesTo<AndroidNotificationSender>().AsSingle();
#endif
        }

        private void BindAnalytics()
        {
            Container.BindInterfacesTo<EmailSenderAnalyticsStrategy>().AsSingle();
            Container.BindInterfacesTo<RedirectAnalyticsStrategy>().AsSingle();
            Container.BindInterfacesTo<SupportAnalyticsStrategy>().AsSingle();
            Container.BindInterfacesTo<QuizAnalyticsStrategy>().AsSingle();
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