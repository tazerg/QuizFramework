using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Zenject;

namespace QuizFramework.EmailSender
{
    public class EmailSenderToSelf : IEmailSenderToSelf, IInitializable, IDisposable
    {
        private EmailSendResult _emailSendResult;
        
        private readonly IEmailSenderToSelfConfig _config;
        private readonly SmtpClient _smtpClient;

        public EmailSenderToSelf(IEmailSenderToSelfConfig config)
        {
            _config = config;

            _smtpClient = new SmtpClient(_config.SmtpHost, _config.SmtpPort)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                DeliveryFormat = SmtpDeliveryFormat.International,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_config.EmailAddress, _config.EmailSecret)
            };
        }

        private void Initialize()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            
            _smtpClient.SendCompleted += OnSendCompleted;
        }

        private void Dispose()
        {
            _smtpClient.SendCompleted -= OnSendCompleted;
        }

        private async Task<EmailSendResult> SendEmail(string message)
        {
            _emailSendResult = EmailSendResult.None;
            var mailMessage = new MailMessage(_config.EmailAddress, _config.EmailAddress)
            {
                Subject = _config.MessageSubject,
                Body = message,
                IsBodyHtml = false,
            };

            try
            {
                await _smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception e)
            {
                OnSendCompleted(null, new AsyncCompletedEventArgs(e, true, null));
            }
            
            return _emailSendResult;
        }

        private void OnSendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null)
            {
                _emailSendResult = EmailSendResult.Error;
                return;
            }

            _emailSendResult = EmailSendResult.Success;
        }

        #region IEmailSenderToSelf

        async Task<EmailSendResult> IEmailSenderToSelf.SendEmail(string message)
        {
            return await SendEmail(message);
        }

        #endregion

        #region IInitializable

        void IInitializable.Initialize()
        {
            Initialize();
        }

        #endregion

        #region IDisposable

        void IDisposable.Dispose()
        {
            Dispose();
        }

        #endregion
    }
}