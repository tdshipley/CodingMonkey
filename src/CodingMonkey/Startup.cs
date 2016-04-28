namespace skeleton_navigation_es2016_vs
{
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices.ComTypes;
    using System.Threading.Tasks;

    using CodingMonkey.Models;

    using Microsoft.AspNet.Authentication.Cookies;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Extensions.PlatformAbstractions;

    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Hosting;
    using Microsoft.AspNet.Http;
    using Microsoft.Data.Entity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
