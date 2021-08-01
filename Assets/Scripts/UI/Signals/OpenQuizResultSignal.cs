namespace QuizFramework.UI.Signals
{
    public struct OpenQuizResultSignal
    {
        public int CorrectAnswersCount { get; }

        public OpenQuizResultSignal(int correctAnswersCount)
        {
            CorrectAnswersCount = correctAnswersCount;
        }
    }
}