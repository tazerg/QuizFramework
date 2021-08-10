namespace QuizFramework.Analytics
{
    public interface IQuizAnalyticsStrategy
    {
        void QuestionsGroupSelectedEvent(ushort selectedGroup);
        void QuestionsGroupPassedEvent(ushort passedGroup, int maxPassedGroup, int correctAnswers, int maxQuestions, bool isGroupPassed);
    }
}