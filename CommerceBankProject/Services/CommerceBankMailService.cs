using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceBankProject.Services
{
    public class CommerceBankMailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public CommerceBankMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = _configuration["CommerceBankEmailKey"];
            var client = new SendGridClient(new SendGridClientOptions { ApiKey = apiKey, HttpErrorAsException = true });
            var from = new EmailAddress("cbwebdesk@gmail.com", "Commerce Bank Web Desk");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);

        }
    }
}
