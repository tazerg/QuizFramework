using System.Threading.Tasks;

namespace QuizFramework.EmailSenderToSelf
{
    public interface IEmailSenderToSelf
    {
        Task<EmailSendResult> SendEmail(string message);
    }
}