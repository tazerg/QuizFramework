using QuizFramework.Analytics;
using QuizFramework.LocalConfig;
using QuizFramework.UI.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace QuizFramework.UI
{
    public class MainMenuVM : BaseWindowVM<OpenMainMenuSignal>
    {
        [Inject] private ISocialNetworkConfig SocialNetworkConfig { get; set; }
        [Inject] private IRedirectAnalyticsStrategy RedirectAnalytics { get; set; }
        
        [SerializeField] private Button _selectLevelButton;
        [SerializeField] private Button _moreGamesButton;
        [SerializeField] private Button _vkLinkButton;
        [SerializeField] private Button _sendQuestionButton;
        [SerializeField] private Button _supportButton;
        [SerializeField] private Button _exitButton;

        protected override void OnInitialize()
        {
            _selectLevelButton.onClick.AddListener(OnSelectLevelButtonCLicked);
            _moreGamesButton.onClick.AddListener(OnMoreGamesButtonClicked);
            _vkLinkButton.onClick.AddListener(OnVkLinkButtonClicked);
            _sendQuestionButton.onClick.AddListener(OnSendQuestionButtonClicked);
            _supportButton.onClick.AddListener(OnSupportButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        protected override void OnDispose()
        {
            _selectLevelButton.onClick.RemoveListener(OnSelectLevelButtonCLicked);
            _moreGamesButton.onClick.RemoveListener(OnMoreGamesButtonClicked);
            _vkLinkButton.onClick.RemoveListener(OnVkLinkButtonClicked);
            _sendQuestionButton.onClick.RemoveListener(OnSendQuestionButtonClicked);
            _supportButton.onClick.RemoveListener(OnSupportButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        protected override void CheckReferences()
        {
            if (_selectLevelButton == null)
                Debug.LogError("Select level button not set!");
            
            if (_moreGamesButton == null)
                Debug.LogError("More games button not set!");
            
            if (_vkLinkButton == null)
                Debug.LogError("Vk link button not set!");
            
            if (_sendQuestionButton == null)
                Debug.LogError("Send question button not set!");
            
            if (_supportButton == null)
                Debug.LogError("Support button not set!");
            
            if (_exitButton == null) 
                Debug.LogError("Exit button not set!");
        }
        
        protected override void OnHandleEvent(OpenMainMenuSignal eventParams)
        {
        }

        private void OnSelectLevelButtonCLicked()
        {
            Close();
            SignalBus.Fire(new OpenSelectLevelSignal());
        }

        private void OnMoreGamesButtonClicked()
        {
            RedirectAnalytics.ReportOpenMoreGamesEvent();
            Application.OpenURL(SocialNetworkConfig.DeveloperPageLink);
        }

        private void OnVkLinkButtonClicked()
        {
            RedirectAnalytics.ReportOpenSocialNetworkEvent("vk");
            Application.OpenURL(SocialNetworkConfig.VkLink);
        }

        private void OnSendQuestionButtonClicked()
        {
            Close();
            SignalBus.Fire(new OpenSendQuestionSignal());
        }

        private void OnSupportButtonClicked()
        {
            Close();
            SignalBus.Fire(new OpenSupportSignal());
        }

        private void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}