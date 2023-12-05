namespace UseAuthTest.Models.ViewModels;

public class LoginViewModel
{
    public UserModel UserRegistration { get; set; } = null!;
    public UserModel UserAuthorization { get; set; } = null!;
    public bool IsRegisterForm { get; set; }
}