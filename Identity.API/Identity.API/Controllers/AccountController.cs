using Identity.API.Models;
using Identity.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Identity.API.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public IActionResult Login() => View();
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
            {
                ModelState.AddModelError("", "Email or Password is incorrect");
                return View(model);
            }
        }
        return View(model);
    }

    public IActionResult Register() => View();
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = new User()
            {
                FullName = model.Name,
                Email = model.Email,
                UserName = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
                return RedirectToAction("Login", "Account");
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }
        }
        return View(model);
    }

    public IActionResult VerifyEmail() => View();
    [HttpPost]
    public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user is null)
            {
                ModelState.AddModelError("", "Something is wrong!");
                return View(model);
            }
            else
                return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
        }
        return View();
    }

    public IActionResult ChangePassword(string username)
    {
        if (string.IsNullOrEmpty(username))
            return RedirectToAction("VerifyEmail", "Account");
        return View(new ChangePasswordViewModel() { Email = username });
    }
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user is not null)
            {
                var result = await _userManager.RemovePasswordAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddPasswordAsync(user, model.NewPassword);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Not Found");
                return View(model);
            }
        }
        else
        {
            ModelState.AddModelError("", "Something is wrong, try again");
            return View(model);
        }
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
