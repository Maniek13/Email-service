using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmailService.DbModels
{
    internal class Email
    {
        [Key]
        public long Id { get; set; }
        [DefaultValue(false)]
        public bool WasSended { get; set; } = false;
        [Required]
        public string? From { get; set; }

        public string? DisplayName { get; set; }
        [Required]
        public string To { get; set; }
        public string Bcc { get; set; }

        public string Cc { get; set; }
        [Required]
        public string? ReplyTo { get; set; }

        public string? ReplyToName { get; set; }
        [Required]
        public string? Subject { get; set; }
        [Required]
        public string? Body { get; set; }
        [DefaultValue(0)]
        public int Trials { get; set; } = 0;

    }
}
