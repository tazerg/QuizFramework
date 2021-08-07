using QuizFramework.UI.Signals;
using UnityEngine;

namespace QuizFramework.UI
{
    public class AnswerButton : BaseQuizButton
    {
        private byte _answerIndex;

        public void SetText(string answer)
        {
            Text.text = answer;
            Image.color = Color.white;
        }

        public void SetInteractable(bool isInteractable)
        {
            Button.interactable = isInteractable;
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
            Image.color = isCorrect ? Color.green : Color.red;
        }
    }
}