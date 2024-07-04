using DAL.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) 
        { 
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            LoginViewModel viewModel = new();
            if (User.Identity == null)
            {
                return View(viewModel);
            }

            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("User") || User.IsInRole("Admin"))
                {
                    return Redirect("/home");
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user;

                user = await _userManager.FindByNameAsync(viewModel.Username) ?? await _userManager.FindByEmailAsync(viewModel.Username);

                if (user == null)
                {
                    ModelState.AddModelError("Login", "Username or password is invalid");
                    return View(viewModel);
                }

                if (await _userManager.CheckPasswordAsync(user, viewModel.Password))
                {
                    Microsoft.AspNetCore.Identity.SignInResult loginResult = await _signInManager.PasswordSignInAsync(user, viewModel.Password, false, false);

                    if (loginResult.Succeeded)
                    {
                        return Redirect("/home");
                    }
                    else
                    {
                        ModelState.AddModelError("Login", "Login failed, please try again.");
                        return View(viewModel);
                    }
                }
                else
                {
                    ModelState.AddModelError("Login", "Username or password is invalid");
                    return View(viewModel);
                }
            }
            ModelState.AddModelError("Login", "Critical error has occurd.");
            return View(viewModel);
        }
    }
}
