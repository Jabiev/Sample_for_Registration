﻿using System.ComponentModel.DataAnnotations;

namespace Identity.API.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
    [DataType(DataType.Password)]
    [Compare(nameof(ConfirmPassword), ErrorMessage = "Password doesn't match")]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }
}
