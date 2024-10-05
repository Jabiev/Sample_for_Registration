using Microsoft.AspNetCore.Identity;

namespace Identity.API.Models;

public class User : IdentityUser
{
    public string FullName { get; set; }
}
