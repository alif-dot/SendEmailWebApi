using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using SendEmailWebApi.Models;
using Microsoft.Extensions.Options;

namespace SendEmailWebApi.Services
{
    public class AttachmentsEmailService : IAttachmentsEmailService
    {
        private readonly EmailSettings emailSettings;
        public AttachmentsEmailService(IConfiguration configuration)
        {
            emailSettings = new EmailSettings
            {
                Email = configuration["EmailSettings:Email"],
                Password = configuration["EmailSettings:Password"],
                Host = configuration["EmailSettings:Host"],
                Displayname = configuration["EmailSettings:Displayname"],
                Port = int.Parse(configuration["EmailSettings:Port"])
            };
        }

        public async Task SendEmailAsync(Mailrequest mailrequest)
        {
            if (mailrequest == null)
            {
                throw new ArgumentNullException(nameof(mailrequest), "Mailrequest must not be null");
            }

            var email = new MimeMessage();

            if (string.IsNullOrWhiteSpace(emailSettings?.Email))
            {
                throw new ArgumentNullException(nameof(emailSettings.Email), "Email address must not be null or empty");
            }

            email.Sender = MailboxAddress.Parse(emailSettings.Email);

            if (string.IsNullOrWhiteSpace(mailrequest.ToEmail))
            {
                throw new ArgumentNullException(nameof(mailrequest.ToEmail), "ToEmail must not be null or empty");
            }

            email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));
            email.Subject = mailrequest.Subject;
            var builder = new BodyBuilder();

            byte[] fileBytes;
            if (File.Exists("Attachment/dummy.pdf"))
            {
                FileStream file = new FileStream("Attachment/dummy.pdf", FileMode.Open, FileAccess.Read);
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                builder.Attachments.Add("attachment.pdf", fileBytes, ContentType.Parse("application/octet-stream"));
                builder.Attachments.Add("attachment2.pdf", fileBytes, ContentType.Parse("application/octet-stream"));
            }

            builder.HtmlBody = mailrequest.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
