using UnityEngine;

namespace QuizFramework.LocalConfig
{
    [CreateAssetMenu(fileName = "SocialNetworkConfig", menuName = "Quiz/Social Network Config")]
    public class SocialNetworkConfig : ScriptableObject, ISocialNetworkConfig
    {
        [SerializeField] private string _vkLink;
        [SerializeField] private string _developerPageLink;
        [SerializeField] private string _gamePageLink;

        #region ISocialNetworkConfig

        string ISocialNetworkConfig.VkLink => _vkLink;
        string ISocialNetworkConfig.DeveloperPageLink => _developerPageLink;
        string ISocialNetworkConfig.GamePageLink => _gamePageLink;

        #endregion
    }
}