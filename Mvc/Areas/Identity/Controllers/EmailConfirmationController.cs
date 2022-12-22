using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mvc.Areas.Identity.Models;
using System.Text.Json;

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
                var model = new EmailConfirmViewModel
                {
                    Email = email,
                };
                return View("Index", model);
            }
            else
            {
                var model = new EmailConfirmViewModel
                {
                    Email = email,
                    Status = true
                };
                ModelState.AddModelError("", "Beklenmedik bir hata olustu, daha sonra tekrar deneyiniz");
                return View("Index", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendMail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Email", new { token, email = email }, Request.Scheme);
            EmailHelper emailHelper = new EmailHelper();
            bool isConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            bool emailResponse = false;
            if (!isConfirmed)
            {
                emailResponse = emailHelper.SendEmail(email, confirmationLink);
            }
            if (emailResponse)
            {
                var model = JsonSerializer.Serialize(new EmailConfirmViewModel
                {
                    Email = email,
                    Status = isConfirmed,
                });
                return Json(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
