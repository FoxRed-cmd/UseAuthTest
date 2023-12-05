using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace UseAuthTest.ViewComponents;

public class MainMenuViewComponent : ViewComponent
{
    private readonly IHttpContextAccessor httpContext;
    public MainMenuViewComponent(IHttpContextAccessor httpContext)
    {
        this.httpContext = httpContext;
    }

    public IViewComponentResult Invoke()
    {
        var isAuthenticated = httpContext.HttpContext?.User?.Identity?.IsAuthenticated;
        if(isAuthenticated != null && (bool)isAuthenticated == true)
            return View("Index", (bool)isAuthenticated);
        return View("Index", false);
    }
}