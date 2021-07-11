using System;
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
        
        [SerializeField] private Button _selectLevelButton;
        [SerializeField] private Button _moreGamesButton;
        [SerializeField] private Button _vkLinkButton;
        [SerializeField] private Button _sendQuestionButton;
        [SerializeField] private Button _supportButton;
        
        protected override void OnHandleEvent(OpenMainMenuSignal eventParams)
        {
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

        protected override void Initialize()
        {
            _selectLevelButton.onClick.AddListener(OnSelectLevelButtonCLicked);
            _moreGamesButton.onClick.AddListener(OnMoreGamesButtonClicked);
            _vkLinkButton.onClick.AddListener(OnVkLinkButtonClicked);
            _sendQuestionButton.onClick.AddListener(OnSendQuestionButtonClicked);
            _supportButton.onClick.AddListener(OnSupportButtonClicked);
        }

        protected override void Dispose()
        {
            _selectLevelButton.onClick.RemoveListener(OnSelectLevelButtonCLicked);
            _moreGamesButton.onClick.RemoveListener(OnMoreGamesButtonClicked);
            _vkLinkButton.onClick.RemoveListener(OnVkLinkButtonClicked);
            _sendQuestionButton.onClick.RemoveListener(OnSendQuestionButtonClicked);
            _supportButton.onClick.RemoveListener(OnSupportButtonClicked);
        }

        private void OnSelectLevelButtonCLicked()
        {
            throw new NotImplementedException();
        }

        private void OnMoreGamesButtonClicked()
        {
            Application.OpenURL(SocialNetworkConfig.DeveloperPageLink);
        }

        private void OnVkLinkButtonClicked()
        {
            Application.OpenURL(SocialNetworkConfig.VkLink);
        }

        private void OnSendQuestionButtonClicked()
        {
            throw new NotImplementedException();
        }

        private void OnSupportButtonClicked()
        {
            throw new NotImplementedException();
        }
    }
}