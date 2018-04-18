using EmailService.Contracts;
using EmailService.Service;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.Controllers
{
    [Route("api/[controller]")]
    public class WebhookController : Controller
    {
        private readonly IEmailService _emailService;

        public WebhookController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // POST api/webhook
        [HttpPost]
        public void Post([FromBody] Email email)
        {
            //TODO: Implement this!
        }
    }
}