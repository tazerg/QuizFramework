using QuizFramework.EmailSender;
using UnityEngine;

namespace QuizFramework.LocalConfig
{
    [CreateAssetMenu(fileName = "EmailSenderToSelfConfig", menuName = "Quiz/Email Sender To Self Config")]
    public class EmailSenderToSelfConfig : ScriptableObject, IEmailSenderToSelfConfig
    {
        [SerializeField] private string _smtpHost;
        [SerializeField] private int _smtpPort;
        [SerializeField] private string _emailAddress;
        [SerializeField] private string _emailSecret;
        [SerializeField] private string _messageSubject;

        #region IEmailSenderToSelfConfig

        string IEmailSenderToSelfConfig.SmtpHost => _smtpHost;
        int IEmailSenderToSelfConfig.SmtpPort => _smtpPort;
        string IEmailSenderToSelfConfig.EmailAddress => _emailAddress;
        string IEmailSenderToSelfConfig.EmailSecret => _emailSecret;
        string IEmailSenderToSelfConfig.MessageSubject => _messageSubject;

        #endregion
    }
}