using EmailService.Interfaces;
using System.ComponentModel.DataAnnotations;


namespace EmailService.Models
{

    internal class Email : IEmail
    {
        [Required]
        public string? From { get; set; }

        public string? DisplayName { get; set; }
        [Required]
        public List<string>? To { get; set; }
        public List<string>? Bcc { get; set; }

        public List<string>? Cc { get; set; }
        [Required]
        public string? ReplyTo { get; set; }

        public string? ReplyToName { get; set; }
        [Required]
        public string? Subject { get; set; }
        [Required]
        public string? Body { get; set; }
    }
}
