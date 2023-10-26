using EmailService.Controllers;
using EmailService.Models;
using Microsoft.Extensions.Options;

namespace EmailService
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly EmailController _controller;

        public Worker(ILogger<Worker> logger, IOptions<EmailSettings> options)
        {
            _logger = logger;
            _controller = new(options.Value);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Emails service started at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            _logger.LogInformation("Try to send emails at: {time}", DateTimeOffset.Now);
                            int nrOf = await _controller.AsyncSendNotSendedEmails();
                            _logger.LogInformation("Send {nrOf} emails", nrOf);

                            int delNrOf = await _controller.AsyncDeleteEmails();
                            _logger.LogInformation("Delete {delNrOf} emails", nrOf);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "{Message}", ex.Message);
                        }

                        await Task.Delay(TimeSpan.FromHours(2), stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{Message}", ex.Message);
                }
            }
        }
    }
}