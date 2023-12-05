namespace UseAuthTest.BL.Interfaces;

public interface IEncrypt
{
    string HashPassword(string password, Guid salt);

}