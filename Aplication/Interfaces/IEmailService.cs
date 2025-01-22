using EmailService.Models;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
}