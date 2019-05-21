using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Contests;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Exceptions;
using Core.Factories;
using Core.Helpers;
using Core.Managers;
using Core.Models;
using Core.Models.Configs;
using Core.Registration;
using Core.Sessions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Front.React
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });

            var sqlConnectionString = Configuration.GetConnectionString("DataContext");

            services.AddDbContext<Context>(options =>
                options.UseNpgsql(
                    sqlConnectionString,
                    b => b.MigrationsAssembly("AspNet5MultipleProject")
                )
            );

            services.Configure<ConfigOptions>(Configuration);

            ConfigureDI(services);
            RegisterStoredConfigs(services).Wait();
        }

        private void ConfigureDI(IServiceCollection services)
        {
            services.AddSingleton<IUserCookieManager, UserCookieManager>();
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddSingleton<IAuthenticationAccountFactory, AuthenticationAccountFactory>();
            services.AddSingleton<IEmailConfirmationRequestFactory, EmailConfirmationRequestFactory>();
            services.AddSingleton<ICryptoHelper, CryptoHelper>();
            services.AddSingleton<IDataGenerator, DataGenerator>();
            services.AddScoped<IContestManager, ContestManager>();
            services.AddSingleton<IEmailManager, EmailManager>();
            services.AddSingleton<INewsManager, NewsManager>();
            services.AddSingleton<ISecurityManager, SecurityManager>();
            services.AddScoped(typeof(IAsyncRepository<>), typeof(Repository<>));
        }

        private static async Task RegisterStoredConfigs(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var configRepo = scope.ServiceProvider.GetService<IAsyncRepository<StoredConfig>>();

                var configs = await configRepo.ListAllAsync();
                RegisterStoredConfig<EmailConfig>(services, configs);
                RegisterStoredConfig<VkAppConfig>(services, configs);
            }
        }

        private static void RegisterStoredConfig<T>(IServiceCollection services, IReadOnlyList<StoredConfig> storedConfigs)
            where T : class, IConfig
        {
            var typeName = typeof(T).Name;

            var wantedStoredConfigs = storedConfigs.Where(c => c.TypeName == typeName).ToArray();
            if (wantedStoredConfigs.Length != 1)
                throw new ConfigNotConsistentException<T>();

            var storedConfig = JsonConvert.DeserializeObject<T>(wantedStoredConfigs[0].JsonValue);
            services.AddSingleton(storedConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(
                routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller}/{action=Index}/{id?}");
                });

            app.UseSpa(
                spa =>
                {
                    spa.Options.SourcePath = "ClientApp";

                    if (env.IsDevelopment())
                    {
                        spa.UseReactDevelopmentServer(npmScript: "start");
                    }
                });
        }
    }
}