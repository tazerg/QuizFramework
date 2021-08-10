using QuizFramework.LocalConfigs;
using QuizFramework.SignalBus;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace QuizFramework.UI
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public abstract class BaseQuizButton : MonoBehaviour
    {
        [Inject] protected readonly IButtonColorsConfig ButtonColorsConfig;
        [Inject] protected readonly ISignalBus SignalBus;
        
        protected Button Button;
        protected Text Text;
        
        private Image _image;

        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }
            
            _image = GetComponent<Image>();
            Button = GetComponent<Button>();
            Text = GetComponentInChildren<Text>();
            if (Text == null)
                Debug.LogError("Text component not found!");
            
            Button.onClick.AddListener(OnButtonClicked);
            OnAwake();
            _isInitialized = true;
        }

        protected abstract void OnAwake();
        protected abstract void OnButtonClicked();
        
        protected void SetButtonColor(ButtonType buttonType)
        {
            var currentColor = ButtonColorsConfig.GetColor(buttonType);
            _image.color = currentColor;
        }
        
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