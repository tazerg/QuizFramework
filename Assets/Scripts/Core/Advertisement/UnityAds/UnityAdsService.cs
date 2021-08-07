using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

using UnityAds = UnityEngine.Advertisements.Advertisement;

namespace QuizFramework.Advertisement
{
    public class UnityAdsService : IAdsService, IInitializable, IDisposable
    {
        private readonly IAdsConfig _adsConfig;
        
        private readonly IUnityAdsShowListener _adsShowListener;
        private TaskCompletionSource<AdShowResult> _taskCompletionSource;

        public UnityAdsService(IAdsConfig adsConfig)
        {
            _adsConfig = adsConfig;
            
            _adsShowListener = new UnityAdsShowListener();
        }
        
        private void Initialize()
        {
            UnityAds.Initialize(_adsConfig.AdProjectId);
            LoadAllPlacements();

            ((UnityAdsShowListener) _adsShowListener).ShowAdResultReceived += OnAdsShowResultReceived;
        }

        private void Dispose()
        {
            ((UnityAdsShowListener) _adsShowListener).ShowAdResultReceived -= OnAdsShowResultReceived;
        }

        private void LoadAllPlacements()
        {
            var placements = (AdPlacement[]) Enum.GetValues(typeof(AdPlacement));
            foreach (var placement in placements)
            {
                if (placement == AdPlacement.None)
                {
                    continue;
                }

                var placementId = GetPlacementId(placement);
                UnityAds.Load(placementId);
            }
        }
        
        private bool IsReady(AdPlacement placement)
        {
            var placementId = GetPlacementId(placement);
            return UnityAds.IsReady(placementId);
        }

        private async Task<AdShowResult> ShowAd(AdPlacement placement)
        {
            var placementId = GetPlacementId(placement);
            _taskCompletionSource = new TaskCompletionSource<AdShowResult>();
            UnityAds.Show(placementId, _adsShowListener);
#if UNITY_EDITOR
            return AdShowResult.Finished;
#endif
            return await _taskCompletionSource.Task;
        }

        private string GetPlacementId(AdPlacement placement)
        {
            switch (placement)
            {
                case AdPlacement.Rewarded:
                    return _adsConfig.RewardedPlacementId;
                case AdPlacement.Interstitial:
                    return _adsConfig.InterstitialPlacementId;
                default:
                    Debug.LogError($"Not supported placement type {placement}");
                    return string.Empty;
            }
        }

        private void OnAdsShowResultReceived(AdShowResult result)
        {
            _taskCompletionSource.SetResult(result);
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