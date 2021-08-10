using System.Linq;
using QuizFramework.Analytics;
using QuizFramework.InApps;
using QuizFramework.UI.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace QuizFramework.UI
{
    public class SupportVM : BaseWindowVM<OpenSupportSignal>
    {
        [Inject] private readonly IInAppsService _inAppsService;
        [Inject] private readonly IInAppsConfig _inAppsConfig;
        [Inject] private readonly ISupportAnalyticsStrategy _supportAnalytics;
        [Inject] private readonly DiContainer _diContainer;
        
        [Header("Prefabs")]
        [SerializeField] private SupportButton _supportButtonPrefab;
        
        [Header("Inner components")] 
        [SerializeField] private Transform _supportButtonsParent;
        
        [Header("Inner ui elements")] 
        [SerializeField] private Button _backButton;
        
        protected override void OnInitialize()
        {
            SpawnSupportButtons();
            SignalBus.Subscribe<SupportProductSelected>(OnProductSelected);
            
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        protected override void OnDispose()
        {
            SignalBus.Unsubscribe<SupportProductSelected>(OnProductSelected);
            
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
        }

        protected override void CheckReferences()
        {
            if (_supportButtonPrefab == null)
                Debug.LogError("Support button prefab not set!");
            
            if (_supportButtonsParent == null)
                Debug.LogError("Support buttons parent not set!");
            
            if (_backButton == null)
                Debug.LogError("Back button not set!");
        }

        protected override void OnHandleEvent(OpenSupportSignal eventParams)
        {
            if (!TryShowErrorPopup())
            {
                _supportAnalytics.OpenSupportWindowEvent();
                return;
            }
            
            OnBackButtonClicked();
        }

        private void SpawnSupportButtons()
        {
            var inAppInfos = _inAppsConfig.GetInAppInfos();
            foreach (var inAppInfo in inAppInfos)
            {
                var supportButton = Instantiate(_supportButtonPrefab, _supportButtonsParent);
                _diContainer.Inject(supportButton);
                supportButton.Setup(inAppInfo);
            }
        }

        private bool TryShowErrorPopup()
        {
            if (_inAppsService.IsInitialized && _inAppsConfig.GetInAppInfos().Any())
            {
                return false;
            }
            
            SignalBus.Fire(new ShowMessagePopupSignal("Раздел временно недоступен, попробуйте попытку позже!"));
            return true;
        }

        private void OnProductSelected(SupportProductSelected signal)
        {
            _supportAnalytics.StartBuyingProductEvent(signal.ProductId, signal.ProductName);
            _inAppsService.PurchaseProduct(signal.ProductId);
        }
        
        private void OnBackButtonClicked()
        {
            SignalBus.Fire(new OpenMainMenuSignal());
            Close();
        }
    }
}