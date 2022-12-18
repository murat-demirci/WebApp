using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Mvc.Areas.Admin.Models;

namespace Mvc.Areas.Admin.ViewComponents
{
    [Area("Admin")]
    //menuyu dinamik hale getirmek icin yapilir
    //menu diger bilesenlerden bagimsiz olarak calisacak
    //controller gibi dusunulebilir
    public class AdminMenuViewComponent : ViewComponent
    {

        private readonly UserManager<User> _userManager;
        public AdminMenuViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
            //view icinde model kuillnilir, model eklenmeli
        }
        //her bir component bi adet invoke metota sahip olmali
        public ViewViewComponentResult Invoke()
        {
            //controller da view return eder gibi, oncesinde model kullanma/ islem yapma imkani sunar
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            //httpcontext.user hangi kullanici login ise onu getirir.
            var roles = _userManager.GetRolesAsync(user).Result;
            return View(new UserWithRoleViewModel
            {
                User = user,
                Roles = roles
            });
        }
        //sayfa yonlendirmesi icin, shared icine Components klasoru
        //sonra AdminMenu klasoru acilir, (componenet adi ile klasor adi ayni olmali)
        //icerisinde Default.cshtml sayfasi bulunmali
    }
}
