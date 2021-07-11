using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace QuizFramework.Advertisement
{
    public class UnityAdsShowListener : IUnityAdsShowListener
    {
        public event Action<AdShowResult> ShowAdResultReceived;
        
        private void OnUnityAdsShowFailure(UnityAdsShowError error, string message)
        {
            Debug.LogError($"Ad show failed. Error {error}. Message {message}");
            ShowAdResultReceived?.Invoke(AdShowResult.Failed);
        }

        private void OnUnityAdsShowClick()
        {
            ShowAdResultReceived?.Invoke(AdShowResult.Finished);
        }

        private void OnUnityAdsShowComplete(UnityAdsShowCompletionState showCompletionState)
        {
            if (showCompletionState == UnityAdsShowCompletionState.SKIPPED)
            {
                ShowAdResultReceived?.Invoke(AdShowResult.Skipped);
                return;
            }
            
            ShowAdResultReceived?.Invoke(AdShowResult.Finished);
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