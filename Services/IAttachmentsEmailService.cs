namespace SendEmailWebApi.Services
{
    public interface IAttachmentsEmailService
    {
        Task SendEmailAsync(Mailrequest mailrequest);
    }
}
