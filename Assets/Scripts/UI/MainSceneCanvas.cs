using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuizFramework.UI
{
    public class MainSceneCanvas : MonoBehaviour
    {
        private IEnumerable<IWindow> _allWindows;
        
        private void Awake()
        {
            _allWindows = GetComponentsInChildren<IWindow>(true);
            InitializeAllWindows();
            TryOpenMenuWindow();
        }

        private void InitializeAllWindows()
        {
            foreach (var window in _allWindows)
            {
                window.Initialize();
                window.Close();
            }
        }

        private void TryOpenMenuWindow()
        {
            var mainMenuWindow = _allWindows.FirstOrDefault(x => x is MainMenuVM);
            if (mainMenuWindow == null)
            {
                Debug.LogError("Can't find main menu window!");
                return;
            }

            mainMenuWindow.TryOpen();
        }
    }
}