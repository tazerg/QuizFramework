using QuizFramework.UI.Signals;
using UnityEngine;

namespace QuizFramework.UI
{
    public class LevelButton : BaseQuizButton
    {
        private ushort _level;

        public void SetLevel(ushort level)
        {
            _level = level;
            Text.text = level.ToString();
        }

        public void SetPassed(bool isPassed)
        {
            Image.color = isPassed ? Color.green : Color.white;
        }

        public void SetAvailable(bool isAvailable)
        {
            if (_level == 1)
            {
                Button.interactable = true;
                return;
            }
            
            Button.interactable = isAvailable;
        }

        protected override void OnAwake()
        {
        }

        protected override void OnButtonClicked()
        {
            SignalBus.Fire(new LevelSelectedSignal(_level));
        }
    }
}