using System.ComponentModel.DataAnnotations;

namespace Identity.API.ViewModels;

public class VerifyEmailViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; }
}
