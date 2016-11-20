using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Services.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Budget.Id
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDeveloperIdentityServer()
                .AddInMemoryClients(new List<Client>
                {
                    new Client
                    {
                        ClientId = "budget.io",
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        ClientSecrets = new List<Secret>
                        {
                            new Secret("secret".Sha256())
                        },
                        AllowedScopes =
                        {
                            "api"
                        },
                        AllowedCorsOrigins = { "http://shelldn-ubuntu.westeurope.cloudapp.azure.com:4200" }
                    }
                })
                .AddInMemoryScopes(new List<Scope>
                {
                    new Scope
                    {
                        Name = "api"
                    }
                })
                .AddInMemoryUsers(new List<InMemoryUser>
                {
                    new InMemoryUser
                    {
                        Subject = "1",
                        Username = "yevgeny.shirin@gmail.com",
                        Password = "qwerty123"
                    }
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Debug);
            app.UseDeveloperExceptionPage();

            app.UseIdentityServer();
        }
    }
}
