using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using UseAuthTest.BL.Interfaces;

namespace UseAuthTest.BL;

public class Encrypt : IEncrypt
{
    public string HashPassword(string password, Guid salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(password, salt.ToByteArray(), KeyDerivationPrf.HMACSHA512,
        10000, 512 / 8));
    }
}
