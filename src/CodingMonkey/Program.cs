namespace CodingMonkey
{
    using System;
    using CodingMonkey.Configuration;
    using CodingMonkey.Models;
    using CodingMonkey.Models.Repositories;
    using Microsoft.AspNetCore;
	using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public class Program
    {
        // Entry point for the application.
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            EnsureDatabaseMigratedAndSeeded(host);

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .ConfigureAppConfiguration((hostContext, config) =>
                   {
                        config.AddEnvironmentVariables();
                   })
                   .Build();

        private static void EnsureDatabaseMigratedAndSeeded(IWebHost host){
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<CodingMonkeyContext>();
                    var codingMonkeyRepositoryContext = services.GetRequiredService<CodingMonkeyRepositoryContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var initialUserConfig = services.GetRequiredService<IOptions<InitialUserConfig>>();
                    var env = services.GetRequiredService<IHostingEnvironment>();
                    var configuration = services.GetRequiredService<IConfiguration>();
                    var seedData = new CodingMonkeyContextSeedData(context,
                                                                   codingMonkeyRepositoryContext,
                                                                   userManager,
                                                                   initialUserConfig,
                                                                   env);

                    context.Database.EnsureCreatedAsync().Wait();
                    //context.Database.Migrate();
                    seedData.EnsureSeedDataAsync().Wait();
                }
                catch (Exception ex)
                {
                    throw new Exception("Problem seeding database", ex);
                }
            }
        }
    }
}
