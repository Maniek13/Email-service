using EmailService.Controllers;
using EmailService.Helpers;
using EmailService.Interfaces;
using EmailService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace EmailServiceTests.ControllerTests
{

    [TestClass]
    public class EmailControllerTests
    {
        static readonly EmailSettings settings = new();
        public EmailControllerTests()
        {

            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Configuration.Sources.Clear();
            builder.Configuration
                .AddJsonFile($"appsettings.Tests.json", true, true);
            builder.Configuration.GetSection(nameof(EmailSettings))
                .Bind(settings);
        }
        

        readonly IEmaillController emailControllers = new EmailController(settings);

        [TestMethod]
        public async Task SendEmail()
        {
            try
            {

                Email email = new()
                {
                    From = "mani3k1989@gmial.com",
                    DisplayName = "maniek",
                    To = new List<string>() { "mariusz.a.szczerba@gmail.com" },
                    Cc = new List<string>() { "maniekpolska13@gmail.com" },
                    ReplyTo = "mani3k1989@gmial.com",
                    ReplyToName = "repleyname",
                    Subject = "test subject",
                    Body = @"<a style=""color:blue;"">test</a>"
                };
                var msg = EmailHelper.CreateEmail(email);

                await emailControllers.AsyncSendEmail(msg);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}