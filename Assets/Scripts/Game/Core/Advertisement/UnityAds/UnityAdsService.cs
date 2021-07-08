using System;
using System.Threading.Tasks;
using QuizFramework.LocalConfig;
using QuizFramework.SignalBus;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;
using UnityAds = UnityEngine.Advertisements.Advertisement;

namespace QuizFramework.Advertisement
{
    public class UnityAdsService : IAdsService, IInitializable, IDisposable
    {
        private readonly ILocalConfig _localConfig;
        private readonly IUnityAdsShowListener _adsShowListener;
        private readonly ISignalBus _signalBus;

        private readonly TaskCompletionSource<AdShowResult> _taskCompletionSource = new TaskCompletionSource<AdShowResult>();

        public UnityAdsService(ILocalConfig localConfig, IUnityAdsShowListener adsShowListener, ISignalBus signalBus)
        {
            _localConfig = localConfig;
            _adsShowListener = adsShowListener;
            _signalBus = signalBus;
        }
        
        private void Initialize()
        {
            UnityAds.Initialize(_localConfig.AdvertisementId);
            
            _signalBus.Subscribe<ShowAdResultReceived>(OnAdsShowResultReceived);
        }

        private void Dispose()
        {
            _signalBus.Unsubscribe<ShowAdResultReceived>(OnAdsShowResultReceived);
        }

        private bool IsReady(AdPlacement placement)
        {
            var placementId = GetPlacementId(placement);
            return UnityAds.IsReady(placementId);
        }

        private async Task<AdShowResult> ShowAd(AdPlacement placement)
        {
            var placementId = GetPlacementId(placement);
            UnityAds.Show(placementId, _adsShowListener);
            return await _taskCompletionSource.Task;
        }

        private string GetPlacementId(AdPlacement placement)
        {
            switch (placement)
            {
                case AdPlacement.Rewarded:
                    return _localConfig.RewardedPlacementId;
                case AdPlacement.Interstitial:
                    return _localConfig.InterstitialPlacementId;
                default:
                    Debug.LogError($"Not supported placement type {placement}");
                    return string.Empty;
            }
        }

        private void OnAdsShowResultReceived(ShowAdResultReceived signal)
        {
            _taskCompletionSource.SetResult(signal.AdShowResult);
        }

        #region IAdsService

        bool IAdsService.IsReady(AdPlacement placement)
        {
            return IsReady(placement);
        }

        async Task<AdShowResult> IAdsService.ShowAd(AdPlacement placement)
        {
            return await ShowAd(placement);
        }

        #endregion

        #region IInitializable

        void IInitializable.Initialize()
        {
            Initialize();
        }

        #endregion

        #region IDisposable

        void IDisposable.Dispose()
        {
            Dispose();
        }

        #endregion
    }
}