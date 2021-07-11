using System.Linq;
using UnityEngine;

namespace QuizFramework.UI
{
    public class MainSceneCanvas : MonoBehaviour
    {
        private void Awake()
        {
            var allWindows = GetComponentsInChildren<IWindow>(true);
            foreach (var window in allWindows)
            {
                window.Initialize();
                window.Close();
            }

            var mainMenuWindow = allWindows.FirstOrDefault(x => x is MainMenuVM);
            if (mainMenuWindow == null)
            {
                Debug.LogError("Can't find main menu window!");
                return;
            }
            
            mainMenuWindow.TryOpen();
        }
    }
}