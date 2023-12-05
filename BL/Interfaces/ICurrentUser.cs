using UseAuthTest.Models;

namespace UseAuthTest.BL.Interfaces;

public interface ICurrentUser
{
    Task<UserModel?> GetCurrentUser();
    Task UpdateCurrentUser(UserModel model);
}