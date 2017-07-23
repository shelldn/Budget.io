using System.Collections.Generic;
using Budget.Api.Filters;
using Budget.Api.Formatters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace Budget.Api.Configuration
{
    public static class ApiServiceCollectionExtensions
    {
        private static void ConfigureAuthorization(MvcOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.Filters.Add(new AuthorizeFilter(policy));
        }

        private static void ConfigureFilters(FilterCollection filters)
        {
            filters.Add(typeof(JsonApiResultFilter));
        }

        private static void ConfigureFormatters(
            ICollection<IInputFormatter> inputFormatters,
            ICollection<IOutputFormatter> outputFormatters)
        {
            inputFormatters.Add(new JsonApiInputFormatter());
        }

        public static void AddBudgetApi(this IServiceCollection services)
        {
            services.AddMvc(o =>
            {
                ConfigureAuthorization(o);
                // ConfigureFilters(o.Filters);
                // ConfigureFormatters(o.InputFormatters, o.OutputFormatters);

            }).AddJsonOptions(o =>
            {
                o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
        }
    }
}
