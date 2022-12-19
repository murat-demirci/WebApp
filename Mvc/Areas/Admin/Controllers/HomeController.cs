using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mvc.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<User> _userManager;

        public HomeController(UserManager<User> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var wwwroot = _env.WebRootPath;
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var path = Path.Combine($"{wwwroot}/img", user.UserPicture);
                if (!System.IO.File.Exists(path))
                {
                    user.UserPicture = "Default/defaultUser.jpg";
                    await _userManager.UpdateAsync(user);
                }
            }
            return View();
        }
    }
}
