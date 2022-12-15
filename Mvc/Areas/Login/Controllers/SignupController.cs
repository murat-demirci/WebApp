using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mvc.Areas.Login.Controllers
{
    [Area("Login")]
    public class SignupController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        public SignupController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
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
                user.UserPicture = "Default/defaultUser.jpg";
                var result = await _userManager.CreateAsync(user, signupDto.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "Home", new { area = "Admin" });
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                ModelState.AddModelError(string.Empty, "Gecersiz giris islemi");
            }
            return View("Index");
        }
    }
}
