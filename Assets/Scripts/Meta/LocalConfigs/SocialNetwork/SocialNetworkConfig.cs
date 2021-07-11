using UnityEngine;

namespace QuizFramework.LocalConfig
{
    [CreateAssetMenu(fileName = "SocialNetworkConfig", menuName = "Quiz/Social Network Config")]
    public class SocialNetworkConfig : ScriptableObject, ISocialNetworkConfig
    {
        [SerializeField] private string _vkLink;
        [SerializeField] private string _developerPageLink;

        #region ISocialNetworkConfig

        string ISocialNetworkConfig.VkLink => _vkLink;
        string ISocialNetworkConfig.DeveloperPageLink => _developerPageLink;

        #endregion
    }
}