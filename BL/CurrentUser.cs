using UseAuthTest.BL.Interfaces;
using UseAuthTest.BL.Helpers;
using UseAuthTest.DAL.Interfaces;
using UseAuthTest.Models;

namespace UseAuthTest.BL;

public class CurrentUser : ICurrentUser
{
    private readonly IUserDAL userDAL;
    private readonly IHttpContextAccessor httpContext;

    public CurrentUser(IUserDAL userDAL, IHttpContextAccessor httpContext)
    {
        this.userDAL = userDAL;
        this.httpContext = httpContext;
    }

    public async Task<UserModel?> GetCurrentUser()
    {
        if (httpContext is not null && httpContext.HttpContext is not null)
        {
            var current = httpContext.HttpContext.Session.Get<UserModel>("CurrentUser");

            if (current is null)
            {
                var identity = httpContext.HttpContext.User.Identities.FirstOrDefault();
                current = await userDAL.GetByEmailAsync(identity?.Name ?? "");
                httpContext.HttpContext.Session.Set("CurrentUser", current);
            }
            return current;
        }
        return null;
    }

    public async Task UpdateCurrentUser(UserModel model)
    {
        if (httpContext is not null && httpContext.HttpContext is not null)
        {
            await userDAL.UpdateAsync(model);
            httpContext.HttpContext.Session.Set("CurrentUser", model);
        }

    }
}