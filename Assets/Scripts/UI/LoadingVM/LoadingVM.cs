using System;
using QuizFramework.Loading;
using UnityEngine;
using UnityEngine.UI;

namespace QuizFramework.UI
{
    public class LoadingVM : BaseWindowVM<LoadingProgressReportSignal>
    {
        private const float LoadingImageRotationSpeed = 20f;
        
        [SerializeField] private Text _progressStatus;
        [SerializeField] private Text _progressbarText;
        [SerializeField] private Image _progressbarFill;
        [SerializeField] private RectTransform _loadingImage;

        private IProgress<float> _progressbar;

        protected override void OnHandleEvent(LoadingProgressReportSignal eventParams)
        {
            _progressbar.Report(eventParams.Progress);
            SetProgressStatus(eventParams.Status);
        }

        protected override void CheckReferences()
        {
            if (_progressStatus == null)
                Debug.LogError("Progress status not set!");
            
            if (_progressbarText == null)
                Debug.LogError("Progressbar text not set!");
            
            if (_progressbarFill == null)
                Debug.LogError("Progressbar fill not set!");
            
            if (_loadingImage == null)
                Debug.LogError("Loading image not set!");
        }

        protected override void Initialize()
        {
            _progressbar = new Progressbar(_progressbarText, _progressbarFill);
        }

        protected override void Dispose()
        {
        }

        private void SetProgressStatus(string status)
        {
            _progressStatus.text = status;
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