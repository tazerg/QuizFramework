using System.Linq;
using System.Text;
using QuizFramework.EmailSenderToSelf;
using QuizFramework.UI.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace QuizFramework.UI
{
    public class SendQuestionVM : BaseWindowVM<OpenSendQuestionSignal>
    {
        [Inject] private IEmailSenderToSelf EmailSenderToSelf { get; set; }
        
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _sendQuestionButton;
        [SerializeField] private InputField _questionInput;
        [SerializeField] private InputField[] _answerInputs;
        
        protected override void OnInitialize()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _sendQuestionButton.onClick.AddListener(OnSendQuestionButtonCLicked);
        }

        protected override void OnDispose()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
            _sendQuestionButton.onClick.RemoveListener(OnSendQuestionButtonCLicked);
        }

        protected override void CheckReferences()
        {
            if (_backButton == null)
                Debug.LogError("Back button not set!");
            
            if (_sendQuestionButton == null)
                Debug.LogError("Send question button not set!");
            
            if (_questionInput == null)
                Debug.LogError("Question input not set!");
            
            if (_answerInputs == null || _answerInputs.Length == 0)
                Debug.LogError("Answers input not set!");
        }
        
        protected override void OnHandleEvent(OpenSendQuestionSignal eventParams)
        {
            ClearInputFields();
        }

        private void OnBackButtonClicked()
        {
            Close();
            SignalBus.Fire(new OpenMainMenuSignal());
        }

        private async void OnSendQuestionButtonCLicked()
        {
            if (!IsInputsValid())
            {
                return;
            }
            
            SignalBus.Fire(new OpenLoadingSignal());

            var message = CreateMessage();
            var sendResult = await EmailSenderToSelf.SendEmail(message);
            
            SignalBus.Fire(new CloseLoadingSignal());

            var resultPopupMessage = GetPopupResultMessage(sendResult);
            ShowPopup(resultPopupMessage);

            if (sendResult != EmailSendResult.Success)
            {
                return;
            }
            
            ClearInputFields();
        }

        private void ClearInputFields()
        {
            _questionInput.text = string.Empty;

            foreach (var answerInput in _answerInputs)
            {
                answerInput.text = string.Empty;
            }
        }

        private bool IsInputsValid()
        {
            if (string.IsNullOrEmpty(_questionInput.text))
            {
                ShowPopup("Не заполнено поле с вопросом. Пожалуйста, введите вопрос.");
                return false;
            }

            if (_answerInputs.Any(x => string.IsNullOrEmpty(x.text)))
            {
                ShowPopup("Один или несколько вариантов ответа не заполнены. Пожалуйста, введите все варианты ответов.");
                return false;
            }

            return true;
        }

        private string CreateMessage()
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.Append($"{_questionInput.text}\n");

            for (var i = 0; i < _answerInputs.Length; i++)
            {
                messageBuilder.Append($"Ответ{i + 1}: {_answerInputs[i].text}\n");
            }

            return messageBuilder.ToString();
        }

        private void ShowPopup(string message)
        {
            SignalBus.Fire(new ShowSendQuestionPopupSignal(message));
        }

        private string GetPopupResultMessage(EmailSendResult sendResult)
        {
            switch (sendResult)
            {
                case EmailSendResult.Success:
                    return "Спасибо, что отправили нам вопрос! После модерации мы добавим его в базу.";
                case EmailSendResult.Error:
                    return "Что-то пошло не так. Проверьте соединение с интернетом и повторите попытку позже.";
                default:
                    Debug.LogError($"Not supported result type {sendResult}");
                    return string.Empty;
            }
        }
    }
}