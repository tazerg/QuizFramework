using System;
using QuizFramework.Advertisement;
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
        [Inject] private IAdsService AdsService { get; set; }
        
        [SerializeField] private Button _selectLevelButton;
        [SerializeField] private Button _moreGamesButton;
        [SerializeField] private Button _vkLinkButton;
        [SerializeField] private Button _sendQuestionButton;
        [SerializeField] private Button _supportButton;

        protected override void OnInitialize()
        {
            _selectLevelButton.onClick.AddListener(OnSelectLevelButtonCLicked);
            _moreGamesButton.onClick.AddListener(OnMoreGamesButtonClicked);
            _vkLinkButton.onClick.AddListener(OnVkLinkButtonClicked);
            _sendQuestionButton.onClick.AddListener(OnSendQuestionButtonClicked);
            _supportButton.onClick.AddListener(OnSupportButtonClicked);
        }

        protected override void OnDispose()
        {
            _selectLevelButton.onClick.RemoveListener(OnSelectLevelButtonCLicked);
            _moreGamesButton.onClick.RemoveListener(OnMoreGamesButtonClicked);
            _vkLinkButton.onClick.RemoveListener(OnVkLinkButtonClicked);
            _sendQuestionButton.onClick.RemoveListener(OnSendQuestionButtonClicked);
            _supportButton.onClick.RemoveListener(OnSupportButtonClicked);
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
        }
        
        protected override void OnHandleEvent(OpenMainMenuSignal eventParams)
        {
        }

        private async void OnSelectLevelButtonCLicked()
        {
            //FOR TEST
            if (!AdsService.IsReady(AdPlacement.Rewarded))
            {
                Debug.LogError("Ad not ready");
                return;
            }
            
            var adResult = await AdsService.ShowAd(AdPlacement.Rewarded);
            Debug.Log($"Ads result {adResult}");
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
            SignalBus.Fire(new OpenSendQuestionSignal());
            Close();
        }

        private void OnSupportButtonClicked()
        {
            throw new NotImplementedException();
        }
    }
}