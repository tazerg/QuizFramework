using QuizFramework.EmailSender;
using QuizFramework.Storage;

namespace QuizFramework.Analytics
{
    public class EmailSenderAnalyticsStrategy : BaseAnalyticsStrategy, IEmailSenderAnalyticsStrategy
    {
        private const string QuestionSendEventId = "questionSend";
        
        public EmailSenderAnalyticsStrategy(IAnalyticsService analyticsService, ILocalStorage localStorage) : 
            base(analyticsService, localStorage)
        {
        }

        private void ReportEvent(EmailSendResult sendResult, string message)
        {
            var eventArgs = GetGlobalArgs();
            eventArgs.Add("question", message);
            eventArgs.Add("sendResult", sendResult.ToString());
            
            AnalyticsService.SendEvent(QuestionSendEventId, eventArgs);
        }

        #region IEmailSenderAnalyticsStrategy

        void IEmailSenderAnalyticsStrategy.ReportEvent(EmailSendResult sendResult, string message)
        {
            ReportEvent(sendResult, message);
        }

        #endregion
    }
}