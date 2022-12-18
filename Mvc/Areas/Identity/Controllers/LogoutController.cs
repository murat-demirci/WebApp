using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mvc.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class LogoutController : Controller
    {
        private readonly SignInManager<User> _signInManager;

        public LogoutController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        [Authorize]
        //sisteme giris yapan kullanici cikis yapabilir
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}
