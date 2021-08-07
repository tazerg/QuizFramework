namespace QuizFramework.UI.Signals
{
    public struct OpenQuizResultSignal
    {
        public ushort CurrentGroup { get; }
        public int CorrectAnswersCount { get; }
        public int AllQuestionsCount { get; }

        public OpenQuizResultSignal(ushort currentGroup, int correctAnswersCount, int allQuestionsCount)
        {
            CurrentGroup = currentGroup;
            CorrectAnswersCount = correctAnswersCount;
            AllQuestionsCount = allQuestionsCount;
        }
    }
}