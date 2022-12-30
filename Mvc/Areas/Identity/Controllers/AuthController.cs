using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mvc.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AuthController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]

        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync
                        (
                        user,
                        userLoginDto.Password,
                        userLoginDto.RememberMe,
                        false);
                    //sifre ile giris yapmak icin 4. deger sifrenin bir cok kez yanlis girilmesi durumunda hesap kitleme ozelligi
                    bool emailStatus = await _userManager.IsEmailConfirmedAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else if (!emailStatus)
                    {
                        return RedirectToAction("Index", "EmailConfirmation", new { email = userLoginDto.Email });
                    }
                    else
                    {
                        ModelState.AddModelError("", "E-posta adresi veya sifre hatali");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "E-posta adresi veya sifre hatali");
                    return View();
                    //hata isleminden sonra view dondurulmeli
                }
            }
            else
            {
                return View();
            }

        }

        [Authorize]
        [HttpGet]
        public ViewResult AccessDenied()
        {
            return View();
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
