using Budget.Api.Configuration;
using Budget.Api.Formatters;
using Budget.Api.Services;
using Budget.Data;
using Budget.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Budget.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        private void ConfigureDependencies(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<IMonthGenerator, MonthGenerator>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDependencies(services);

            services.AddRouting(o => o.LowercaseUrls = true);

            services.AddCors();

            services.AddBudgetData();
            services.AddBudgetApi();

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddConsole(Configuration.GetSection("Logging"));

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors(b => b
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = env.IsDevelopment() ? "http://localhost:52138" : "http://budget-id.azurewebsites.net",
                AllowedScopes = { "api" },
                RequireHttpsMetadata = false
            });

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUi();
        }
    }
}
