using EmailService.Contracts;
using EmailService.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

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
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:54565");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //var response = client.PostAsJsonAsync("api/email", email).Result;
                //I can use thie statement above, but it will bring in System.Net.Http.Formatting package and dependency, may cause some compatibility issue when maintain

                var stringContent = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
                var response = client.PostAsync("api/email", stringContent).Result;
                if (response.IsSuccessStatusCode)
                {

                }

            }
        }
    }
}