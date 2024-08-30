namespace PTC.Utils;

public interface IEmailSender
{
    Task SendEmailGuest(string? email, string? cc1, string? cc2, string subject, string body, string attachmentPath);
    Task SendEmailEmp(string recipientEmail, string subject, string message);
}
