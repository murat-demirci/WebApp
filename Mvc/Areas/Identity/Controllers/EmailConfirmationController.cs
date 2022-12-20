using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mvc.Areas.Identity.Models;

namespace Mvc.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class EmailConfirmationController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public EmailConfirmationController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Email", new { token, email = email }, Request.Scheme);
            EmailHelper emailHelper = new EmailHelper();
            bool emailResponse = emailHelper.SendEmail(email, confirmationLink);
            if (emailResponse)
            {
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Beklenmedik bir hata olustu, daha sonra tekrar deneyiniz");
                return View();
            }
        }
    }
}
