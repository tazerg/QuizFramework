using QuizFramework.SignalBus;
using QuizFramework.UI.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace QuizFramework.UI
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public class LevelButton : MonoBehaviour
    {
        [Inject] private ISignalBus _signalBus;
        
        private Button _button;
        private Image _image;
        private Text _text;

        private ushort _level;
        private bool _isPassed;
        private bool _isAvailable;

        public void SetLevel(ushort level)
        {
            if (_level == level)
            {
                return;
            }
            
            _level = level;
            _text.text = level.ToString();
        }

        public void SetPassed(bool isPassed)
        {
            if (_isPassed == isPassed)
            {
                return;
            }
            
            _isPassed = isPassed;
            _image.color = isPassed ? Color.green : Color.white;
        }

        public void SetAvailable(bool isAvailable)
        {
            if (_isAvailable == isAvailable)
            {
                return;
            }

            _isAvailable = isAvailable;
            _button.interactable = isAvailable;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<Text>();
            
            _button.onClick.AddListener(OnButtonCLicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonCLicked);
        }

        private void OnButtonCLicked()
        {
            _signalBus.Fire(new LevelSelectedSignal(_level));
        }
    }
}