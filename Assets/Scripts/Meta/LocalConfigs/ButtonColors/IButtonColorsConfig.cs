using UnityEngine;

namespace QuizFramework.LocalConfigs
{
    public interface IButtonColorsConfig
    {
        Color GetColor(ButtonType buttonType);
    }
}