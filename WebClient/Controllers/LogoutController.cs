using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers
{
    public class LogoutController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        public LogoutController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Login");
        }
    }
}
