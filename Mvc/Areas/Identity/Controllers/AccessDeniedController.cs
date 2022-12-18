using Microsoft.AspNetCore.Mvc;

namespace Mvc.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccessDeniedController : Controller
    {
        public IActionResult Unauthorized()
        {
            return View("403");
        }
    }
}
