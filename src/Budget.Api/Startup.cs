using Budget.Api.Configuration;
using Budget.Api.Services;
using Budget.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Budget.Api
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        private static void ConfigureDependencies(IServiceCollection services)
        {
            services.AddScoped<IMonthGenerator, MonthGenerator>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDependencies(services);

            services.AddRouting(o => o.LowercaseUrls = true);

            services.AddCors();

            services.AddBudgetData(connectionString: Configuration.GetConnectionString("DefaultConnection"));
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
                Authority = env.IsDevelopment() ? "http://localhost:52138" : "http://shelldn-ubuntu.westeurope.cloudapp.azure.com:8081",
                AllowedScopes = { "api" },
                RequireHttpsMetadata = false
            });

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUi();
        }
    }
}
