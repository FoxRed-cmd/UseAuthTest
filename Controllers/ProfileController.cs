using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseAuthTest.BL.Interfaces;
using UseAuthTest.Models;
using UseAuthTest.Models.ViewModels;

namespace UseAuthTest.Controllers;

public class ProfileController : Controller
{
    private readonly ICurrentUser currentUser;
    private readonly IEncrypt encrypt;
    private readonly IUserAuth userAuth;
    public ProfileController(ICurrentUser currentUser, IEncrypt encrypt, IUserAuth userAuth)
    {
        this.currentUser = currentUser;
        this.encrypt = encrypt;
        this.userAuth = userAuth;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        return View(await currentUser.GetCurrentUser() ?? new UserModel());
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit()
    {
        var user = await currentUser.GetCurrentUser();
        return View(new ProfileViewModel() { User = user ?? new UserModel() });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(ProfileViewModel model)
    {
        var user = await currentUser.GetCurrentUser();

        if (user is null)
            return NotFound();

        ModelState.Remove("User.Password");

        if (model.OldPassword != null && model.NewPassword != null)
        {
            model.OldPassword = encrypt.HashPassword(model.OldPassword, user.Salt);

            if (model.OldPassword == user.Password)
                user.Password = encrypt.HashPassword(model.NewPassword, user.Salt);
            else
                ModelState.AddModelError("User.Password", "Passwords don't match, please try again");
        }

        if (ModelState.IsValid)
        {
            user.Email = model.User.Email;
            user.FirstName = model.User.FirstName;
            user.SecondName = model.User.SecondName;
            user.NumberPhone = model.User.NumberPhone;

            await currentUser.UpdateCurrentUser(user);
            await userAuth.UserSignOutAsync();
            await userAuth.UserAuthorize(user);

            return RedirectToAction("Index");

        }

        return View("Edit", new ProfileViewModel() { User = model.User });
    }
}