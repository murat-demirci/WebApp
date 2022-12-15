using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mvc.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/Identity/Login/{id}")]
    [Route("/Identity/Login")]
    public class LoginController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public LoginController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, userLoginDto.RememberMe, false);
                    //sifre ile giris yapmak icin 4. deger sifrenin bir cok kez yanlis girilmesi durumunda hesap kitleme ozelligi
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else
                    {
                        ModelState.AddModelError("", "E-posta adresi veya sifre hatali");
                        return View("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "E-posta adresi veya sifre hatali");
                    return View("Index");
                    //hata isleminden sonra view dondurulmeli
                }
            }
            else
            {
                return View("Index");
            }

        }
    }
}
