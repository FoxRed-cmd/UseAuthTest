using System.Security.Cryptography;
using System.Text;

namespace UseAuthTest.Helpers;

public class WebFileHelper
{
    public const string IMAGE_DIR = "wwwroot\\images";

    public static async Task<string> SaveProfileImage(string userName, IFormFile imageFile)
    {
        MD5 mD5 = MD5.Create();
        byte[] hashName = mD5.ComputeHash(Encoding.ASCII.GetBytes(userName));
        userName = Convert.ToHexString(hashName).Substring(0, 4);

        string dir = Path.Combine(IMAGE_DIR, userName);
        if(!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        dir = Path.Combine(dir, imageFile.FileName);

        if(File.Exists(dir))
            File.Delete(dir);

        using (var stream = File.Create(dir))
            await imageFile.CopyToAsync(stream);

        return dir.Replace("wwwroot\\", string.Empty);
    }
}