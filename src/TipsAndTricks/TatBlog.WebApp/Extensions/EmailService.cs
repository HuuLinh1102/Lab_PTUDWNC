using FluentEmail.Core;
using FluentEmail.Smtp;
using System.Net.Mail;
using System.Net;

public class EmailService
{
    private readonly SmtpSender _smtpSender;

    public EmailService(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
    {
        _smtpSender = new SmtpSender(() => new SmtpClient(smtpServer)
        {
            Port = smtpPort,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(smtpUsername, smtpPassword),
            EnableSsl = true,
        });
    }

	public async Task SendAsync(string toEmail, string subject, string body)
    {
        var email = Email
            .From("tailinh01637@gmail.com")
            .To(toEmail)
            .Subject(subject)
            .Body(body);

        await _smtpSender.SendAsync(email);
    }

}
