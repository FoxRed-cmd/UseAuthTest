using UseAuthTest.Models;

namespace UseAuthTest.DAL.Interfaces;

public interface IUserDAL
{
    Task<int> CreateAsync(UserModel model);
    Task<UserModel?> GetByEmailAsync(string email);
    Task UpdateAsync(UserModel model);
}