using System;
using System.Threading.Tasks;
using EmailService.Contracts;
using EmailService.Service;
using Microsoft.Extensions.Logging;
using MockEmailClient;
using Moq;
using Xunit;

namespace EmailServiceTests
{
    public class EmailingServiceTests
    {
        private readonly EmailingService _sut;
        private readonly Mock<IEmailClient> _mockClient;
        private readonly Mock<ILogger<EmailingService>> _mockLogger;

        public EmailingServiceTests()
        {
            _mockClient = new Mock<IEmailClient>();
            _mockLogger = new Mock<ILogger<EmailingService>>();
            _sut = new EmailingService(_mockClient.Object, _mockLogger.Object);
        }

        [Fact]
        public void Should_Send_Emails_to_Email_Client()
        {
            var email = new Email { To = "George", Body = "Very Important!" };

            _sut.SendEmail(email);
            _sut.SendEmail(email);
            _sut.SendEmail(email);
            _sut.SendEmail(email);

            _mockClient.Verify(call => call.SendEmail(email.To, email.Body), Times.Exactly(4));
        }

        [Fact]
        public void Should_Handle_SendEmail_Failure()
        {
            var email = new Email { To = "George", Body = "Very Important!" };
            _mockClient.Setup(call => call.SendEmail(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("Sending error"));

            var result = _sut.SendEmail(email);

            Assert.Equal("Failure.", result);
        }

        //TODO: More tests!
        [Fact]
        public void Should_Handle_Connection_Failure()
        {
            var email = new Email { To = "George", Body = "Very Important!" };

            // assume maxConnections is 10, so that I can test boundary value using 11
            var client = new EmailClient(10);
            var _eService = new EmailingService(client, _mockLogger.Object);

            int numFailure = 0;

            // try concurrency within 5s to test the boundary value using 11   
            Parallel.For(0, 11, i =>
            {
                try
                {
                    var res = _eService.SendEmail(email);

                    if (res == "Failure.")
                    {
                        numFailure++;
                    }

                }
                catch (Exception)
                {

                }
            });


            Assert.True(numFailure > 0, "No Connection error found");
        }


        [Fact]
        public void Should_Handle_Unexpected_Failure()
        {
            var email = new Email { To = "George", Body = "Very Important!" };


            var _mockClient1 = new EmailClient(100);

            var _eService = new EmailingService(_mockClient1, _mockLogger.Object);

            int numFailure = 0;

            Parallel.For(0, 20, i =>
            {
                try
                {
                    _eService.SendEmail(email);
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("Unexpected Error"))
                    {
                        numFailure++;
                    }
                }
            });

            Assert.True(numFailure > 0, "No Connection error found");

        }
    }
}
