using QuizFramework.UI.Signals;
using UnityEngine;

namespace QuizFramework.UI
{
    public class LoadingVM : BaseWindowVM<OpenLoadingSignal>
    {
        private const float LoadingImageRotationSpeed = 20f;
        
        [SerializeField] private RectTransform _loadingImage;

        protected override void OnInitialize()
        {
            SignalBus.Subscribe<CloseLoadingSignal>(Close);
        }

        protected override void OnDispose()
        {
            SignalBus.Unsubscribe<CloseLoadingSignal>(Close);
        }

        protected override void CheckReferences()
        {
            if (_loadingImage == null)
                Debug.LogError("Loading image not set!");
        }

        protected override void OnHandleEvent(OpenLoadingSignal eventParams)
        {
        }
        
        private void Update()
        {
            RotateLoadingImage();
        }

        private void RotateLoadingImage()
        {
            _loadingImage.RotateAround(_loadingImage.position, Vector3.forward, LoadingImageRotationSpeed * Time.deltaTime);
        }
    }
}