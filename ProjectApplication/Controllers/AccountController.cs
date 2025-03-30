using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectApplication.Entities;
using ProjectApplication.Models.ViewModel;

namespace ProjectApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<MyApplicationUser> _userManager;
        private readonly SignInManager<MyApplicationUser> _signInManager;

        public AccountController(UserManager<MyApplicationUser> userManager, SignInManager<MyApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register() => View();
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new MyApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name // Store Name in the database
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl; // Store returnUrl to redirect after login
            return View();
        }
        [HttpPost]
            public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
            {
                if (!ModelState.IsValid)
                    return View(model);

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                    return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "Home");

                ModelState.AddModelError("", "Invalid login attempt");
               return View(model);
            }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
