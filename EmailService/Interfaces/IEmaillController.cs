using MimeKit;

namespace EmailService.Interfaces
{
    internal interface IEmaillController
    {
        public Task<bool> AsyncSendEmail(MimeMessage message, CancellationToken cancellationToken = default);
        public Task<int> AsyncSendNotSendedEmails();
        public Task<int> AsyncDeleteEmails();
    }
}
