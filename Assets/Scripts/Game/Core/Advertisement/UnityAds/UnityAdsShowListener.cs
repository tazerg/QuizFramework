using QuizFramework.SignalBus;
using UnityEngine;
using UnityEngine.Advertisements;

namespace QuizFramework.Advertisement
{
    public class UnityAdsShowListener : IUnityAdsShowListener
    {
        private readonly ISignalBus _signalBus;

        public UnityAdsShowListener(ISignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void OnUnityAdsShowFailure(UnityAdsShowError error, string message)
        {
            Debug.LogError($"Ad show failed. Error {error}. Message {message}");
            _signalBus.Fire(new ShowAdResultReceived(AdShowResult.Failed));
        }

        private void OnUnityAdsShowClick()
        {
            _signalBus.Fire(new ShowAdResultReceived(AdShowResult.Finished));
        }

        private void OnUnityAdsShowComplete(UnityAdsShowCompletionState showCompletionState)
        {
            if (showCompletionState == UnityAdsShowCompletionState.SKIPPED)
            {
                _signalBus.Fire(new ShowAdResultReceived(AdShowResult.Skipped));
                return;
            }
            
            _signalBus.Fire(new ShowAdResultReceived(AdShowResult.Finished));
        }

        #region IUnityAdsShowListener

        void IUnityAdsShowListener.OnUnityAdsShowStart(string placementId)
        {
            //Nothing
        }

        void IUnityAdsShowListener.OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            OnUnityAdsShowFailure(error, message);
        }

        void IUnityAdsShowListener.OnUnityAdsShowClick(string placementId)
        {
            OnUnityAdsShowClick();
        }

        void IUnityAdsShowListener.OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            OnUnityAdsShowComplete(showCompletionState);
        }

        #endregion
    }
}