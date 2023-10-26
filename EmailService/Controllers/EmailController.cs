using EmailService.Helpers;
using EmailService.Interfaces;
using EmailService.Models;
using EmailService.Providers;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EmailService.Controllers
{
    internal class EmailController : IEmaillController
    {
        readonly IEmailSettings _settings;
        readonly EmailServiceDbProvider emailServiceDbProvider = new();

        internal EmailController(IEmailSettings settings)
        {
            _settings = settings;
        }


        public async Task<int> AsyncSendNotSendedEmails()
        {
            try
            {
                var emailToSend = emailServiceDbProvider.GetNotSendedEmails();

                if (emailToSend == null)
                    return 0;

                int nrOf = 0;
                for (int i = 0; i < emailToSend.Count; i++)
                {
                    Email email = EmailHelper.ConvertToEmail(emailToSend[i]);

                    var mail = EmailHelper.CreateEmail(email);
                    var succes = await AsyncSendEmail(mail);

                    if (succes)
                    {
                        await emailServiceDbProvider.AsyncSetToSended(emailToSend[i].Id);
                        nrOf += 1;
                    }
                    else
                        await emailServiceDbProvider.AsyncAddTry(emailToSend[i].Id);
                }

                return nrOf;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public async Task<bool> AsyncSendEmail(MimeMessage message, CancellationToken cancellationToken = default)
        {
            using var smtp = new SmtpClient();

            try
            {
                smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                await smtp.ConnectAsync(_settings.Host, Int32.Parse(_settings.Port), SecureSocketOptions.SslOnConnect, cancellationToken);
                await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, cancellationToken);
                await smtp.SendAsync(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                await smtp.DisconnectAsync(true, cancellationToken);
            }
        }

        public async Task<int> AsyncDeleteEmails()
        {
            try
            {
                return await emailServiceDbProvider.AsyncDeleteOldEmails(_settings.NrOfTrialsToExpired);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
    }
}
