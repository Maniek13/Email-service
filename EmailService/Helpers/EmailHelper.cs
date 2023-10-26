using EmailService.Interfaces;
using EmailService.Models;
using MimeKit;
using DbEmail = EmailService.DbModels.Email;

namespace EmailService.Helpers
{
    internal class EmailHelper
    {
        internal static MimeMessage CreateEmail(IEmail email)
        {
            try
            {
                CheckIsEmailOkey(email);
                MimeMessage mail = new();

                mail.From.Add(new MailboxAddress(email.DisplayName ?? email.From, email.From));
                mail.Sender = new MailboxAddress(email.DisplayName ?? email.From, email.From);


                foreach (string mailAddress in email.To)
                {
                    if (!string.IsNullOrEmpty(mailAddress))
                        mail.To.Add(MailboxAddress.Parse(mailAddress));
                }


                if (!string.IsNullOrEmpty(email.ReplyTo))
                    mail.ReplyTo.Add(new MailboxAddress(email.ReplyToName ?? email.ReplyTo, email.ReplyTo));


                if (email.Bcc != null)
                {
                    foreach (string mailAddress in email.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                        mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }


                if (email.Cc != null)
                {
                    foreach (string mailAddress in email.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                        mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }

                var body = new BodyBuilder();
                mail.Subject = email.Subject;
                body.HtmlBody = email.Body;
                mail.Body = body.ToMessageBody();

                return mail;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        internal static Email ConvertToEmail(DbEmail email)
        {
            List<string> To = email.To.Trim().Split(',').ToList();
            List<string> Bcc = email.Bcc.Trim().Split(',').ToList();
            List<string> Cc = email.Cc.Trim().Split(',').ToList();



            return new Email()
            {
                From = email.From,
                DisplayName = email.DisplayName,
                To = To,
                Bcc = Bcc,
                Cc = Cc,
                ReplyTo = email.ReplyTo,
                ReplyToName = email.ReplyToName,
                Subject = email.Subject,
                Body = email.Body
            };

        }

        internal static void CheckIsEmailOkey(IEmail email)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));

            if (string.IsNullOrEmpty(email.From))
                throw new ArgumentException("Not set from");

            if (email.To == null)
                throw new ArgumentException(nameof(email.To));

            if (email.To.Count == 0)
                throw new ArgumentException("Not set to");

            if (string.IsNullOrEmpty(email.ReplyTo))
                throw new ArgumentException("Not set reply to");

            if (string.IsNullOrEmpty(email.Subject))
                throw new ArgumentException("Not set subject");

            if (string.IsNullOrEmpty(email.Body))
                throw new ArgumentException("Not set body");
        }
    }
}
