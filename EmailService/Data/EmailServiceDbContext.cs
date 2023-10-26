using EmailService.DbModels;
using Microsoft.EntityFrameworkCore;

namespace EmailService.Data
{
    internal class EmailServiceDbContext : DbContext
    {
        public DbSet<Email> Emails { get; set; }

        public EmailServiceDbContext(DbContextOptions<EmailServiceDbContext> options) : base(options)
        {
        }
    }
}
