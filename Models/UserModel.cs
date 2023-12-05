using System.ComponentModel.DataAnnotations;

namespace UseAuthTest.Models;

public class UserModel
{
    [Key]
    public int Id { get; set; }
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    public Guid Salt { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string SecondName { get; set; } = string.Empty;
    [Required(ErrorMessage = "phone number is incorrect")]
    [RegularExpression(@"^\+\d{1,2}\(\d{3}\)\d{3}-\d{2}-\d{2}$", ErrorMessage = "phone number is incorrect")]
    public string NumberPhone { get; set; } = string.Empty;
    public string Image { get; set; } = "/images/empty-profile/profile-picture.png";

}