using UseAuthTest.Models;

namespace UseAuthTest.BL.Interfaces;

public interface IUserAuth
{
    Task UserRegister(UserModel model);
    Task<bool> UserAuthorize(UserModel model);
    Task<bool> EmailExists (string email);
    Task UserSignOutAsync();
}