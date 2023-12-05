using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseAuthTest.BL.Interfaces;
using UseAuthTest.Middleware;
using UseAuthTest.Models;
using UseAuthTest.Models.ViewModels;

namespace UseAuthTest.Controllers;

public class LoginController : Controller
{
    private readonly IUserAuth userAuth;

    public LoginController(IUserAuth userAuth)
    {
        this.userAuth = userAuth;
    }

    [NonAuthorized]
    public IActionResult Index()
    {
        return View(new LoginViewModel(){IsRegisterForm = true});
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserModel model)
    {
        if(model.Password == null || model.Password == string.Empty)
            ModelState.TryAddModelError("UserRegistration.Password", "Password is required");

        if(model.Email == null || model.Email == string.Empty)
            ModelState.TryAddModelError("UserRegistration.Email", "Email is required");
        else if (await userAuth.EmailExists(model.Email))
            ModelState.TryAddModelError("UserRegistration.Email", "Email already exists");

        if(ModelState.IsValid)
        {
            await userAuth.UserRegister(model);
            return Redirect("/");
        }
        return View("Index", new LoginViewModel(){UserRegistration = model, IsRegisterForm = true});
    }

    [HttpPost]
    public async Task<IActionResult> Authorize(UserModel model)
    {
        if (!await userAuth.EmailExists(model.Email))
            ModelState.TryAddModelError("UserAuthorization.Email", "Email or Password is incorrect");
        if(model.Password == null || model.Password == string.Empty)
            ModelState.TryAddModelError("UserAuthorization.Email", "Email or Password is incorrect");

        ModelState.Remove(nameof(model.NumberPhone));

        if(ModelState.IsValid)
        {
            if(await userAuth.UserAuthorize(model))
                return Redirect("/");
            ModelState.TryAddModelError("UserAuthorization.Email", "Email or Password is incorrect");
            return View("Index", new LoginViewModel(){UserAuthorization = model});
        }
        return View("Index", new LoginViewModel(){UserAuthorization = model, IsRegisterForm = false});
    }

    [Authorize]
    public async Task<IActionResult> UserSignOut()
    {
        await userAuth.UserSignOutAsync();
        return Redirect("/Login");
    }
}