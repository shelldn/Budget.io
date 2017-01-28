﻿using System.Collections.Generic;
using Budget.Data;
using Budget.Id.Services;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Budget.Id
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBudgetData(connectionString: Configuration.GetConnectionString("DefaultConnection"));

            var builder = services
                .AddIdentityServer()
                .AddTemporarySigningCredential()
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
                        AllowedCorsOrigins =
                        {
                            "http://shelldn-ubuntu.westeurope.cloudapp.azure.com",
                            "http://shelldn-ubuntu.westeurope.cloudapp.azure.com:4200"
                        }
                    }
                })
                .AddInMemoryApiResources(new List<ApiResource>
                {
                    new ApiResource("api")
                })
                /*
                .AddTestUsers(new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "5831db9c46c7cae8980e4a56",
                        Username = "yevgeny.shirin@gmail.com",
                        Password = "qwerty123"
                    },
                    new TestUser
                    {
                        SubjectId = "588b61c00bb389ded4c1f369",
                        Username = "july.taranenko@gmail.com",
                        Password = "qwerty123"
                    }
                })
                */
                ;

            builder.Services.AddTransient<IProfileService, MongoProfileService>();
            builder.Services.AddTransient<IResourceOwnerPasswordValidator, MongoResourceOwnerPasswordValidator>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Debug);
            app.UseDeveloperExceptionPage();

            app.UseIdentityServer();
        }
    }
}
