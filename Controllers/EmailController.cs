using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;
using MailKit.Net.Smtp;
using SendEmailWebApi.Models;
using SendEmailWebApi.Services;

namespace SendEmailWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IAttachmentsEmailService _aEmailService;

        public EmailController(IEmailService emailService, IAttachmentsEmailService aEmailService)
        {
            _emailService = emailService;
            _aEmailService = aEmailService;
        }

        [HttpPost("SendEmail")]
        public IActionResult SendEmail(EmailDto request)
        {
            _emailService.SendEmail(request);
            return Ok();
        }

        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail()
        {
            try
            {
                Mailrequest mailrequest = new Mailrequest();
                mailrequest.ToEmail = "malif8636@gmail.com";
                mailrequest.Subject = "Welcome to UY System Ltd";
                mailrequest.Body = GetHtmlcontent();
                await _aEmailService.SendEmailAsync(mailrequest);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetHtmlcontent()
        {
            string Response = "<div style=\"width:100%;background-color:lightblue;text-align:center;margin:10px\">";
            Response += "<h1>Welcome to UY System Ltd.</h1>";
            Response += "<img src=\"https://th.bing.com/th/id/OIP.Ydz0WgJPm_vPBWAYEP5epwHaC0?pid=ImgDet&rs=1\" />";
            Response += "<h2>Thanks for stay with us</h2>";
            Response += "<a href=\"https://twitter.com/AlifEasin\">Please follow by click the link</a>";
            Response += "<div><h1> Contact us : malif8636@gmail.com</h1></div>";
            Response += "</div>";
            return Response;
        }
    }
}
