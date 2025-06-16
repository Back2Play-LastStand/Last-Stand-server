using MimeKit;
using MailKit.Net.Smtp;
using Server.Service.Interface;

namespace Server.Service;

public class EmailService :  IEmailService
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPass;
    private readonly string _fromEmail;
    private readonly string _fromName;
    
    public EmailService(IConfiguration config)
    {
        _smtpHost = config["Smtp:Host"];
        _smtpPort = int.Parse(config["Smtp:Port"]);
        _smtpUser = config["Smtp:User"];
        _smtpPass = config["Smtp:Pass"];
        _fromEmail = config["Smtp:FromEmail"];
        _fromName = config["Smtp:FromName"];
    }
    
    public async Task SendAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_fromName, _fromEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        
        message.Body = new TextPart("html") {
            Text = $"<p>Last Stand 인증코드는 <span style=\"font-size:24px; font-weight:bold;\">{body}</span> 입니다.</p>"
        };
        
        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_smtpUser, _smtpPass);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}