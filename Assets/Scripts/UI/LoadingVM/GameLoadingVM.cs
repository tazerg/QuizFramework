using System;
using QuizFramework.Loading;
using UnityEngine;
using UnityEngine.UI;

namespace QuizFramework.UI
{
    public class GameLoadingVM : BaseWindowVM<LoadingProgressReportSignal>
    {
        [SerializeField] private Text _progressStatus;
        [SerializeField] private Text _progressbarText;
        [SerializeField] private Image _progressbarFill;

        private IProgress<float> _progressbar;

        protected override void OnInitialize()
        {
            _progressbar = new Progressbar(_progressbarText, _progressbarFill);
        }

        protected override void OnDispose()
        {
        }

        protected override void CheckReferences()
        {
            if (_progressStatus == null)
                Debug.LogError("Progress status not set!");
            
            if (_progressbarText == null)
                Debug.LogError("Progressbar text not set!");
            
            if (_progressbarFill == null)
                Debug.LogError("Progressbar fill not set!");
        }

        protected override void OnHandleEvent(LoadingProgressReportSignal eventParams)
        {
            _progressbar.Report(eventParams.Progress);
            SetProgressStatus(eventParams.Status);
        }

        private void SetProgressStatus(string status)
        {
            _progressStatus.text = status;
        }
    }
}