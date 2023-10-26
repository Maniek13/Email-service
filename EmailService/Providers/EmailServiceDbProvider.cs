using EmailService.Data;
using EmailService.Models;
using Microsoft.EntityFrameworkCore;
using DbEmail = EmailService.DbModels.Email;

namespace EmailService.Providers
{
    internal class EmailServiceDbProvider
    {
        private EmailServiceDbContext _dbContext;

        private DbContextOptions<EmailServiceDbContext> GetAllOptions()
        {
            DbContextOptionsBuilder<EmailServiceDbContext> optionsBuilder = new();

            optionsBuilder.UseSqlServer(AppSettings.ConnectionString);

            return optionsBuilder.Options;
        }


        public List<DbEmail> GetNotSendedEmails()
        {
            using (_dbContext = new EmailServiceDbContext(GetAllOptions()))
            {
                try
                {
                    var emails = _dbContext.Emails.Where(el => el.WasSended == false).ToList();

                    if (emails == null)
                        throw new InvalidOperationException("No emails data is found!");

                    return emails;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<bool> AsyncSetToSended(long id)
        {
            using (_dbContext = new EmailServiceDbContext(GetAllOptions()))
            {
                try
                {
                    DbEmail? email = _dbContext.Emails.Where(el => el.Id == id).FirstOrDefault();
                    if (email == null)
                        throw new Exception($"No email with id: {id}");

                    email.WasSended = true;
                    await _dbContext.SaveChangesAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        public async Task<bool> AsyncAddTry(long id)
        {
            using (_dbContext = new EmailServiceDbContext(GetAllOptions()))
            {
                try
                {
                    DbEmail? email = _dbContext.Emails.Where(el => el.Id == id).FirstOrDefault();

                    if (email == null)
                        throw new Exception($"No email with id: {id}");

                    email.Trials += 1;
                    await _dbContext.SaveChangesAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        public async Task<int> AsyncDeleteOldEmails(int nrOfTrialsToExpired)
        {
            using (_dbContext = new EmailServiceDbContext(GetAllOptions()))
            {
                try
                {

                    List<DbEmail>? emails = _dbContext.Emails.Where(el => el.WasSended || el.Trials > nrOfTrialsToExpired).ToList();

                    if (emails == null || emails.Count == 0)
                        return 0;


                    _dbContext.RemoveRange(emails);
                    await _dbContext.SaveChangesAsync();

                    return emails.Count;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }
    }
}
