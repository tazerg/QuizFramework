using System.Threading.Tasks;

namespace QuizFramework.EmailSender
{
    public interface IEmailSenderToSelf
    {
        Task<EmailSendResult> SendEmail(string message);
    }
}