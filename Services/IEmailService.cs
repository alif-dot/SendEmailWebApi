using SendEmailWebApi.Models;

namespace SendEmailWebApi.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}
