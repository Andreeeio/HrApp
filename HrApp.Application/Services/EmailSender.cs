using HrApp.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;


namespace HrApp.Application.Services;

public class EmailSender(IConfiguration configuration) : IEmailSender
{
    private readonly IConfiguration _configuration = configuration;
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        DotNetEnv.Env.Load();

        var smtpHost = _configuration["Smtp:Host"];
        var smtpPort = int.Parse(_configuration["Smtp:Port"]!);
        var smtpUser = _configuration["Smtp:Username"];
        var smtpPass = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
        var fromEmail = _configuration["Smtp:From"];

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true
        };
        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail!),
            Subject = subject,
            Body = message,
            IsBodyHtml = false
        };
        mailMessage.To.Add(email);

        await client.SendMailAsync(mailMessage);
    }
}
