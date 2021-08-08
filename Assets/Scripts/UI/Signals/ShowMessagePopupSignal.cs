namespace QuizFramework.UI.Signals
{
    public struct ShowMessagePopupSignal
    {
        public string Message { get; }

        public ShowMessagePopupSignal(string message)
        {
            Message = message;
        }
    }
}