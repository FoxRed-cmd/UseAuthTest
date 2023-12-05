namespace UseAuthTest.Models.ViewModels;

public class ProfileViewModel
{
    public UserModel User { get; set; } = null!;
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
}