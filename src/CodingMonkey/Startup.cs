﻿namespace CodingMonkey
{
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    using CodingMonkey.Configuration;
    using CodingMonkey.Models;

    using Microsoft.AspNet.Authentication.Cookies;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Extensions.PlatformAbstractions;

    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Hosting;
    using Microsoft.Data.Entity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Serilog;
    using Serilog.Sinks.RollingFile;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            string applicationPath = PlatformServices.Default.Application.ApplicationBasePath;

            // Create SeriLog
            Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Debug()
                                .WriteTo.RollingFile(Path.Combine(applicationPath, "log_{Date}.txt"))
                                .CreateLogger();

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.secrets.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            // Add Db
            var path = PlatformServices.Default.Application.ApplicationBasePath;
            var connection = $"Filename={Path.Combine(path, "codingmonkey.db")}";

            services.AddEntityFramework()
                .AddSqlite()
                .AddDbContext<CodingMonkeyContext>(
                    options =>
                    { options.UseSqlite(connection); });

            services.AddIdentity<ApplicationUser, IdentityRole>(
                identityContext =>
                    {
                        identityContext.Password.RequireDigit = false;
                        identityContext.Password.RequireUppercase = false;
                        identityContext.Password.RequiredLength = 6;

                        identityContext.Cookies
                                       .ApplicationCookie
                                       .Events = new CookieAuthenticationEvents()
                                                    {
                                                        OnRedirectToLogin = ctx =>
                                                            {
                                                                if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                                                                {
                                                                    ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                                                }
                                                                else
                                                                {
                                                                    ctx.Response.Redirect(ctx.RedirectUri);
                                                                }
                                                                return Task.FromResult(0);
                                                            }
                                                    };
                    })
                .AddEntityFrameworkStores<CodingMonkeyContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.Configure<InitialUserConfig>(
                user =>
                    {
                        user.Email = Configuration["InitialUser:Email"];
                        user.UserName = Configuration["InitialUser:UserName"];
                        user.Password = Configuration["InitialUser:Password"];
                    });

            services.Configure<IdentityServerConfig>(
                config =>
                    {
                        config.ClientId = Configuration["IdentityServer:ClientId"];
                        config.ClientSecret = Configuration["IdentityServer:ClientSecret"];
                    });

            services.Configure<AppConfig>(
                config =>
                    {
                        config.CodeExecutorApiEndpoint = Configuration["CodeExecutorApiEndpoint"];
                        config.IdentityServerApiEndpoint = Configuration["IdentityServerEndpoint"];
                    });

            services.AddTransient<CodingMonkeyContextSeedData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, CodingMonkeyContextSeedData seeder, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseIdentity();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            await seeder.EnsureSeedDataAsync();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
