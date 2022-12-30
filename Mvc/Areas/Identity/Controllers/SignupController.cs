using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mvc.Areas.Identity.Models;

namespace Mvc.Areas.Identity.Controllers
{
    [Area("Identity")]

    public class SignupController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        public SignupController(UserManager<User> userManager, SignInManager<User> signInManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signup(SignupDto signupDto)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(signupDto);
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.Picture = "~/img/UserImages/defaultUser.jpg";
                //isim sayisim eklenecek
                var result = await _userManager.CreateAsync(user, signupDto.Password);
                List<string> roles = new List<string> { "Article.Create", "Article.Read", "AdminArea.Home.Read" };
                await _userManager.AddToRolesAsync(user, roles);
                if (user != null)
                {
                    if (result.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action("ConfirmEmail", "Email", new { token, email = user.Email }, Request.Scheme);
                        EmailHelper emailHelper = new EmailHelper();
                        bool emailResponse = emailHelper.SendEmail(user.Email, confirmationLink);
                        if (emailResponse)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return RedirectToAction("index", "Home", new { area = "Admin" });
                        }
                        else
                        {
                            return View("Index");
                        }
                    }
                    else
                    {
                        foreach (var err in result.Errors)
                        {
                            ModelState.AddModelError("", err.Description);
                        }

                        return View("Index", signupDto);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "hata");
                    return View("Index");
                }
            }
            return View("Index");
        }
    }
}
