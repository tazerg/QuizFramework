using QuizFramework.Storage;

namespace QuizFramework.Analytics
{
    public class QuizAnalyticsStrategy : BaseAnalyticsStrategy, IQuizAnalyticsStrategy
    {
        private const string QuestionsGroupSelectedEventId = "questionsGroupSelected";
        private const string QuestionsGroupPassedEventId = "questionsGroupPassed";
        
        public QuizAnalyticsStrategy(IAnalyticsService analyticsService, ILocalStorage localStorage) : 
            base(analyticsService, localStorage)
        {
        }

        private void QuestionsGroupSelectedEvent(ushort selectedGroup)
        {
            var eventArgs = GetGlobalArgs();
            eventArgs.Add("selectedGroup", selectedGroup);
            
            AnalyticsService.SendEvent(QuestionsGroupSelectedEventId, eventArgs);
        }

        private void QuestionsGroupPassedEvent(ushort passedGroup, int maxPassedGroup, int correctAnswers, int maxQuestions, bool isGroupPassed)
        {
            var eventArgs = GetGlobalArgs();
            eventArgs.Add("passedGroup", passedGroup);
            eventArgs.Add("correctAnswers", correctAnswers);
            eventArgs.Add("maxQuestions", maxQuestions);
            eventArgs.Add("isGroupPassed", isGroupPassed);
            eventArgs.Add("maxPassedGroup", maxPassedGroup);
            
            AnalyticsService.SendEvent(QuestionsGroupPassedEventId, eventArgs);
        }
        
        #region IQuizAnalyticsStrategy

        void IQuizAnalyticsStrategy.QuestionsGroupSelectedEvent(ushort selectedGroup)
        {
            QuestionsGroupSelectedEvent(selectedGroup);
        }

        void IQuizAnalyticsStrategy.QuestionsGroupPassedEvent(ushort passedGroup, int maxPassedGroup, int correctAnswers, int maxQuestions, bool isGroupPassed)
        {
            QuestionsGroupPassedEvent(passedGroup, maxPassedGroup, correctAnswers, maxQuestions, isGroupPassed);
        }

        #endregion
    }
}