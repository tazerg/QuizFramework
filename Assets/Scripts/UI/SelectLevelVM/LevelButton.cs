using QuizFramework.LocalConfigs;
using QuizFramework.UI.Signals;

namespace QuizFramework.UI
{
    public class LevelButton : BaseQuizButton
    {
        private bool _isPassed;
        private bool _isAvailable;
        private ushort _level;

        public void SetLevel(ushort level)
        {
            _level = level;
            Text.text = level.ToString();
        }

        public void Setup(bool isAvailable, bool isPassed)
        {
            SetAvailable(isAvailable);
            SetPassed(isPassed);
            SetButtonColor();
        }

        protected override void OnAwake()
        {
        }

        protected override void OnButtonClicked()
        {
            SignalBus.Fire(new LevelSelectedSignal(_level));
        }
        
        private void SetPassed(bool isPassed)
        {
            _isPassed = isPassed;
        }

        private void SetAvailable(bool isAvailable)
        {
            if (_level == 1)
            {
                _isAvailable = true;
                Button.interactable = true;
                return;
            }

            _isAvailable = isAvailable;
            Button.interactable = isAvailable;
        }

        private void SetButtonColor()
        {
            if (_isPassed)
            {
                SetButtonColor(ButtonType.Correct);
                return;
            }

            var currentButtonType = _isAvailable ? ButtonType.Active : ButtonType.Inactive;
            SetButtonColor(currentButtonType);
        }
    }
}