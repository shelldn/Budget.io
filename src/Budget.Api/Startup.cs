using Budget.Api.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IMongoClient>(
                sp => new MongoClient(sp.GetService<IConfiguration>().GetConnectionString("DefaultConnection")));

            var pack = new ConventionPack { new CamelCaseElementNameConvention() };

            ConventionRegistry.Register("CamelCase", pack, t => true);

            services.AddTransient(sp => sp.GetService<IMongoClient>().GetDatabase("budgetio"));

            services.AddRouting(o => o.LowercaseUrls = true);

            services.AddCors();

            services.AddMvc(o =>
            {
                o.InputFormatters.Insert(0, new JsonApiInputFormatter());
            });

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(b => b
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://budgetid.azurewebsites.net",
                ScopeName = "api",
                RequireHttpsMetadata = false
            });

            app.UseMvcWithDefaultRoute();

            app.UseSwagger();
            app.UseSwaggerUi();
        }
    }
}
