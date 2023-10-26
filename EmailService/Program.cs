using EmailService.Data;
using EmailService.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EmailServiceTests")]
namespace EmailService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    AppSettings.ConnectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
                    var optionsBuilder = new DbContextOptionsBuilder<EmailServiceDbContext>();
                    optionsBuilder.UseSqlServer(AppSettings.ConnectionString);


                    services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
                    services.AddScoped<EmailServiceDbContext>(db => new EmailServiceDbContext(optionsBuilder.Options));
                    services.AddWindowsService();
                    services.AddHostedService<Worker>();

                })
                .Build();

            CreateDbIfNoneExist(host);

            host.Run();
        }
        private static void CreateDbIfNoneExist(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider;

                try
                {
                    var context = service.GetRequiredService<EmailServiceDbContext>();
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}