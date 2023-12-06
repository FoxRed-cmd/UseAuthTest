using Dapper;
using Npgsql;
using UseAuthTest.DAL.Helpers;
using UseAuthTest.DAL.Interfaces;
using UseAuthTest.Models;

namespace UseAuthTest.DAL;

public class UserDAL : IUserDAL
{
    public async Task<int> CreateAsync(UserModel model)
    {
        using var connection = new NpgsqlConnection(HelperDb.ConnectionString);
        await connection.OpenAsync();
        var result = await connection.QueryFirstOrDefaultAsync<int>(@"
                INSERT INTO ""User"" (Email, Password, Salt)
                VALUES (@Email, @Password, @Salt) RETURNING Id;", model);
        return result;
    }

    public async Task<UserModel?> GetByEmailAsync(string email)
    {
        using var connection = new NpgsqlConnection(HelperDb.ConnectionString);
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<UserModel>(@"
            SELECT * FROM ""User""
            WHERE Email = @Email", new { Email = email });
    }

    public async Task UpdateAsync(UserModel model)
    {
        using var connection = new NpgsqlConnection(HelperDb.ConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(@"
            UPDATE ""User"" SET
            Email = @Email,
            Password = @Password,
            FirstName = @FirstName,
            SecondName = @SecondName,
            NumberPhone = @NumberPhone,
            Image = @Image
            WHERE Id = @Id;", model);
    }
}