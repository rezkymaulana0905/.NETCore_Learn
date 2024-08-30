using System.Net;
using System.Net.Mail;
namespace PTC.Utils;

public class EmailSender(IConfiguration configuration) : IEmailSender
{
    private readonly string _email = configuration.GetValue<string>("MailSettings:Email") ?? throw new Exception("Email configuration value is null.");
    private readonly string _password = configuration.GetValue<string>("MailSettings:Password") ?? throw new Exception("Password configuration value is null.");
    private readonly string _server = configuration.GetValue<string>("MailSettings:Server") ?? throw new Exception("Server configuration value is null.");
    private readonly int _port = configuration.GetValue<int>("MailSettings:Port");
    public async Task SendEmailGuest(string recipientEmail, string? recipientCc1, string? recipientCc2, string subject, string message, string? attachmentPath = null)
    {        
        try
        {
            using var client = new SmtpClient(_server, _port);
            client.EnableSsl = false;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_email, _password);

            var mailMessage = CreateMailMessage(_email, recipientEmail, subject, message);

            if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
            {
                AttachFile(mailMessage, attachmentPath);
            }

            if (!string.IsNullOrEmpty(recipientCc1))
            {
                mailMessage.CC.Add(recipientCc1);
            }

            if (!string.IsNullOrEmpty(recipientCc2) && recipientCc1 != recipientCc2)
            {
                mailMessage.CC.Add(recipientCc2);
            }

            await client.SendMailAsync(mailMessage);

            mailMessage.Attachments.Dispose();
            client.Dispose();

            if (!string.IsNullOrEmpty(attachmentPath))
            {
                File.Delete(attachmentPath);
            }

        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task SendEmailEmp (string recipientEmail, string subject, string message)
    {
        using var client = new SmtpClient(_server, _port);
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(_email, _password);

        var mailMessage = new MailMessage(_email, recipientEmail, subject, message);

        await client.SendMailAsync(mailMessage);
    }
    private static MailMessage CreateMailMessage(string senderEmail, string recipientEmail, string subject, string message)
    {
        var mailMessage = new MailMessage(senderEmail, recipientEmail, subject, message)
        {
            IsBodyHtml = true
        };
        return mailMessage;
    }

    private static void AttachFile(MailMessage mailMessage, string attachmentPath)
    {
        Attachment attachment = new(attachmentPath);
        mailMessage.Attachments.Add(attachment);
    }
}