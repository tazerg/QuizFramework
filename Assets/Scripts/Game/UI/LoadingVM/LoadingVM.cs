using System;
using UnityEngine;
using UnityEngine.UI;

namespace QuizFramework.UI
{
    public class LoadingVM : MonoBehaviour
    {
        private const float LoadingImageRotationSpeed = 20f;
        
        [SerializeField] private Text _progressStatus;
        [SerializeField] private Text _progressbarText;
        [SerializeField] private Image _progressbarFill;
        [SerializeField] private RectTransform _loadingImage;

        private IProgress<float> _progressbar;

        public IProgress<float> Progressbar
        {
            get
            {
                if (_progressbar == null)
                    _progressbar = new Progressbar(_progressbarText, _progressbarFill);

                return _progressbar;
            }
        }

        public void SetProgressStatus(string status)
        {
            _progressStatus.text = status;
        }

        private void Awake()
        {
            CheckReferences();
        }

        private void Update()
        {
            RotateLoadingImage();
        }

        private void CheckReferences()
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

        private void RotateLoadingImage()
        {
            _loadingImage.RotateAround(_loadingImage.position, Vector3.right, LoadingImageRotationSpeed * Time.deltaTime);
        }
    }
}