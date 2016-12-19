using Budget.Api.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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

        private static void ConfigureFilters(FilterCollection filters)
        {
            filters.Add(typeof(JsonApiResultFilter));
        }

        public static void AddApi(this IServiceCollection services)
        {
            services.AddMvc(o =>
            {
                ConfigureAuthorization(o);
                ConfigureFilters(o.Filters);
            });
        }
    }
}