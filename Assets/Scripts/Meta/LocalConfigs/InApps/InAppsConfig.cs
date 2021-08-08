using System.Collections.Generic;
using QuizFramework.InApps;
using UnityEngine;

namespace QuizFramework.LocalConfigs
{
    [CreateAssetMenu(fileName = "InAppsConfig", menuName = "Quiz/In Apps Config")]
    public class InAppsConfig : ScriptableObject, IInAppsConfig
    {
        [SerializeField] private List<InAppInfo> _inAppInfos;

        #region IInAppsConfig

        IEnumerable<InAppInfo> IInAppsConfig.GetInAppInfos()
        {
            return _inAppInfos;
        }

        #endregion
    }
}