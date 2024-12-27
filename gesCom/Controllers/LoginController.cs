using Microsoft.AspNetCore.Mvc;
using gesCom.Models;
using gesCom.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace gesCom.Controllers;

public class LoginController : Controller
{
    private readonly IAuthService _authService;

    public LoginController(IAuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Index()
    {
        ViewData["HideNavBar"] = true;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _authService.AuthenticateAsync(model.Email, model.Password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Email ou mot de passe incorrect.");
        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Login");
    }
}