using EmailService.Interfaces;

namespace EmailService.Models
{
    internal class EmailSettings : IEmailSettings
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public int NrOfTrialsToExpired { get; set; }
    }
}
