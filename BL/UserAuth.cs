using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using UseAuthTest.BL.Interfaces;
using UseAuthTest.DAL.Interfaces;
using UseAuthTest.Models;

namespace UseAuthTest.BL;

public class UserAuth : IUserAuth
{
    private readonly IUserDAL userDAL;
    private readonly IHttpContextAccessor httpContext;
    private readonly IEncrypt encrypt;

    public UserAuth(IUserDAL userDAL, IHttpContextAccessor httpContext, IEncrypt encrypt)
    {
        this.userDAL = userDAL;
        this.httpContext = httpContext;
        this.encrypt = encrypt;
    }

    public async Task UserRegister(UserModel model)
    {
        model.Salt = Guid.NewGuid();
        string temp = model.Password;
        model.Password = encrypt.HashPassword(model.Password, model.Salt);

        int id = await userDAL.CreateAsync(model);

        model.Password = temp;

        await UserAuthorize(model);
    }

    public async Task<bool> EmailExists(string email) => await userDAL.GetByEmailAsync(email) != null;

    public async Task<bool> UserAuthorize(UserModel model)
    {
        var user = await userDAL.GetByEmailAsync(model.Email);

        if(user is null)
            return false;

        if(model.Password != user.Password)
            model.Password = encrypt.HashPassword(model.Password, user?.Salt ?? throw new NullReferenceException());

        if(user.Password == model.Password)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.Email) };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            if(httpContext is not null && httpContext.HttpContext is not null)
            {
                await httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return true;
            }
        }
        return false;
    }

    public async Task UserSignOutAsync()
    {
        if(httpContext is not null && httpContext.HttpContext is not null)
        {
            await httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            httpContext.HttpContext.Session.Clear();
        }
    }
}