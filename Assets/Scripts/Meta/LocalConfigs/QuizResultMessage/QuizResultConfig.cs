using System.Collections.Generic;
using UnityEngine;

namespace QuizFramework.LocalConfigs
{
    [CreateAssetMenu(fileName = "QuizResultConfig", menuName = "Quiz/Quiz Result Config")]
    public class QuizResultConfig : ScriptableObject, IQuizResultConfig
    {
        [SerializeField] private List<QuizResultMessageInfo> _quizResultMessages;
        [SerializeField] private float _minResultRatioToGroupPass;

        private string GetResultMessage(float resultRatio)
        {
            var resultInfo = _quizResultMessages.Find(x => resultRatio > x.MinRatio && resultRatio <= x.MaxRatio);
            return resultInfo.Message;
        }

        private bool IsGroupPassed(float resultRatio)
        {
            return resultRatio >= _minResultRatioToGroupPass;
        }

        #region IQuizResultConfig

        string IQuizResultConfig.GetResultMessage(float resultRatio)
        {
            return GetResultMessage(resultRatio);
        }

        bool IQuizResultConfig.IsGroupPassed(float resultRatio)
        {
            return IsGroupPassed(resultRatio);
        }

        #endregion
    }
}