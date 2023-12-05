using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UseAuthTest.Middleware;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class NonAuthorizedAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var isAuthenticated = context.HttpContext?.User?.Identity?.IsAuthenticated;
        if(isAuthenticated != null && (bool)isAuthenticated == true)
            context.Result = new RedirectResult("/");
    }
}