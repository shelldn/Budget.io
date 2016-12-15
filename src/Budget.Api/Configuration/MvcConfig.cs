using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Budget.Api.Configuration
{
    public static class MvcConfig
    {
        private static void ConfigureAuthorization(MvcOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            // options.Filters.Add(new AuthorizeFilter(policy));
        }

        private static void ConfigureFormatters(MvcOptions options)
        {

        }

        public static void AddApi(this IServiceCollection services)
        {
            services.AddMvc(o =>
            {
                ConfigureAuthorization(o);
                ConfigureFormatters(o);
            });
        }
    }
}