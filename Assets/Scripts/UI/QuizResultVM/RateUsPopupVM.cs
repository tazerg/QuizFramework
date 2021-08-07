using QuizFramework.LocalConfig;
using QuizFramework.UI.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace QuizFramework.UI
{
    public class RateUsPopupVM : BaseWindowVM<ShowRateUsPopupSignal>
    {
        [Inject] private readonly ISocialNetworkConfig _socialNetworkConfig;

        [SerializeField] private Button _okButton;
        [SerializeField] private Button _closeButton;
        
        protected override void OnInitialize()
        {
            _okButton.onClick.AddListener(OnOkButtonClicked);
            _closeButton.onClick.AddListener(Close);
        }

        protected override void OnDispose()
        {
            _okButton.onClick.RemoveListener(OnOkButtonClicked);
            _closeButton.onClick.RemoveListener(Close);
        }

        protected override void CheckReferences()
        {
            if (_okButton == null)
                Debug.LogError("Ok button not set!");
            
            if (_closeButton == null)
                Debug.LogError("Close button not set!");
        }

        protected override void OnHandleEvent(ShowRateUsPopupSignal eventParams)
        {
        }

        private void OnOkButtonClicked()
        {
            Application.OpenURL(_socialNetworkConfig.GamePageLink);
        }
    }
}