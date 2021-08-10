using QuizFramework.LocalConfigs;
using QuizFramework.UI.Signals;

namespace QuizFramework.UI
{
    public class AnswerButton : BaseQuizButton
    {
        private byte _answerIndex;

        public void SetText(string answer)
        {
            Text.text = answer;
        }

        public void SetInteractable(bool isInteractable)
        {
            Button.interactable = isInteractable;
            
            var buttonType = isInteractable ? ButtonType.Active : ButtonType.Inactive;
            SetButtonColor(buttonType);
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        protected override void OnAwake()
        {
            _answerIndex = (byte) transform.GetSiblingIndex();
        }

        protected override void OnButtonClicked()
        {
            SignalBus.Fire(new AnswerSelectedSignal(_answerIndex, AnswerSelectedCallback));
        }

        private void AnswerSelectedCallback(bool isCorrect)
        {
            var buttonType = isCorrect ? ButtonType.Correct : ButtonType.Incorrect;
            SetButtonColor(buttonType);
        }
    }
}