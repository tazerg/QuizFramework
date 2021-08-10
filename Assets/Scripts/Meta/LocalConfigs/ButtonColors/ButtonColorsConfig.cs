using System;
using UnityEngine;

namespace QuizFramework.LocalConfigs
{
    [CreateAssetMenu(fileName = "ButtonColorsConfig", menuName = "Quiz/Button Colors Config")]
    public class ButtonColorsConfig : ScriptableObject, IButtonColorsConfig
    {
        [SerializeField] private Color _activeButtonColor;
        [SerializeField] private Color _inactiveButtonColor;
        [SerializeField] private Color _correctButtonColor;
        [SerializeField] private Color _incorrectButtonColor;

        private Color GetColor(ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.Active:
                    return _activeButtonColor;
                case ButtonType.Inactive:
                    return _inactiveButtonColor;
                case ButtonType.Correct:
                    return _correctButtonColor;
                case ButtonType.Incorrect:
                    return _incorrectButtonColor;
                default:
                    throw new ArgumentException($"Not supported button type {buttonType}");
            }
        }

        #region IButtonColorsConfig

        Color IButtonColorsConfig.GetColor(ButtonType buttonType)
        {
            return GetColor(buttonType);
        }

        #endregion
    }
}