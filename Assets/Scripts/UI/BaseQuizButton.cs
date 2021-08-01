using QuizFramework.SignalBus;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace QuizFramework.UI
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public abstract class BaseQuizButton : MonoBehaviour
    {
        [Inject] protected readonly ISignalBus SignalBus;
        
        protected Button Button;
        protected Image Image;
        protected Text Text;

        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }
            
            Button = GetComponent<Button>();
            Image = GetComponent<Image>();
            Text = GetComponentInChildren<Text>();
            if (Text == null)
                Debug.LogError("Text component not found!");
            
            Button.onClick.AddListener(OnButtonClicked);
            OnAwake();
            _isInitialized = true;
        }

        protected abstract void OnAwake();
        protected abstract void OnButtonClicked();
        
        private void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Button.onClick.RemoveListener(OnButtonClicked);
        }
    }
}