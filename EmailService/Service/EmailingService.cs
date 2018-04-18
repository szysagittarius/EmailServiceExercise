using System;
using System.Threading;
using EmailService.Contracts;
using Microsoft.Extensions.Logging;
using MockEmailClient;

namespace EmailService.Service
{
    public class EmailingService : IEmailService
    {
        private readonly ILogger<EmailingService> _logger;
        private readonly IEmailClient _emailClient;

        public EmailingService(IEmailClient emailClient, ILogger<EmailingService> logger)
        {
            _emailClient = emailClient;
            _logger = logger;
        }

        public string SendEmail(Email email)
        {
            _logger.LogInformation($"Sending email to {email.To}");
            try
            {
                _emailClient.SendEmail(email.To, email.Body);
                return "Success!";
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error sending email to {email.To}");
                return "Failure.";
            }
            finally
            {
                _emailClient.Close();
            }
        }
    }
}
