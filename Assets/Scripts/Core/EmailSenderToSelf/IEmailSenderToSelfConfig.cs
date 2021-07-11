namespace QuizFramework.EmailSenderToSelf
{
    public interface IEmailSenderToSelfConfig
    {
        string SmtpHost { get; }
        int SmtpPort { get; }
        string EmailAddress { get; }
        string EmailSecret { get; }
        string MessageSubject { get; }
    }
}