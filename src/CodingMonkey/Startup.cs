﻿namespace CodingMonkey
{
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    using CodingMonkey.Configuration;
    using CodingMonkey.Models;
    using Microsoft.Extensions.PlatformAbstractions;

    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Serilog;
    using Serilog.Sinks.RollingFile;

    using AutoMapper;

    public class Startup
    {
        private MapperConfiguration _mapperConfiguration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            string applicationPath = env.ContentRootPath;

            // Create SeriLog
            Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Debug()
                                .WriteTo.RollingFile(Path.Combine(applicationPath, "log_{Date}.txt"))
                                .CreateLogger();

            // Setup automapper
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CodingMonkeyAutoMapperProfile());
            });

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(applicationPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.secrets.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                // Not using anyway atm - disabled until https://github.com/aspnet/UserSecrets/issues/62 is fixed
                // builder.AddUserSecrets();

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

            services.AddDbContext<CodingMonkeyContext>();

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

            services.AddSingleton<IMapper>(x => _mapperConfiguration.CreateMapper());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CodingMonkeyContextSeedData seeder)
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
    }
}
