using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Budget.Api.Filters.Extensions
{
    internal static class FilterContextExtensions
    {
        public static UrlHelper GetUrlHelper(this FilterContext context)
        {
            return new UrlHelper(new ActionContext(context.HttpContext, context.RouteData, context.ActionDescriptor));
        }
    }
}