using QuizFramework.EmailSender;

namespace QuizFramework.Analytics
{
    public interface IEmailSenderAnalyticsStrategy
    {
        void ReportEvent(EmailSendResult sendResult, string message);
    }
}