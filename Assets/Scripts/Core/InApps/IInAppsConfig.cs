using System.Collections.Generic;

namespace QuizFramework.InApps
{
    public interface IInAppsConfig
    {
        IEnumerable<InAppInfo> GetInAppInfos();
    }
}