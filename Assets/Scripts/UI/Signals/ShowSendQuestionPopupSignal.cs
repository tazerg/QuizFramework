namespace QuizFramework.UI.Signals
{
    public struct ShowSendQuestionPopupSignal
    {
        public string Message { get; }

        public ShowSendQuestionPopupSignal(string message)
        {
            Message = message;
        }
    }
}