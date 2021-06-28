using System;
using UnityEngine.UI;

namespace QuizFramework.UI
{
    public class Progressbar : IProgress<float>
    {
        private const char PercentSymbol = '%'; 
        private const float PercentMultiplier = 100f;

        private readonly Text _progressText;
        private readonly Image _progressImage;

        public Progressbar(Text progressText, Image progressImage)
        {
            _progressText = progressText;
            _progressImage = progressImage;
        }
        
        private void Report(float progress)
        {
            SetProgressbarText(progress);
            SetProgressbarFill(progress);
        }

        private void SetProgressbarText(float progress)
        {
            var progressPercent = progress * PercentMultiplier;
            _progressText.text = $"{progressPercent}{PercentSymbol}";
        }

        private void SetProgressbarFill(float progress)
        {
            _progressImage.fillAmount = progress;
        }

        #region IProgressbar

        void IProgress<float>.Report(float value)
        {
            Report(value);
        }

        #endregion
    }
}