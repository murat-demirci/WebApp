using AutoMapper;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mvc.Helpers.Abstract;

namespace Mvc.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        //sadece bu controller'ı miras alan controller'ların kullanabilmesi için protected olarak tanımlanır.
        protected UserManager<User> UserManager { get; }
        protected IMapper Mapper { get; }
        protected IImageHelper ImageHelper { get; }
        //loggedinuser'ın controller'a geldiğinde otomatik olarak set edilmesi için aşağıdaki şekilde tanımlanır.
        //bu sayede base controller'dan üretilmiş controller'larda login olmuş kullanıcının bilgisine ulaşabiliriz
        protected User LoggedInUser => UserManager.GetUserAsync(HttpContext.User).Result;

        public BaseController(UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper)
        {
            UserManager = userManager;
            Mapper = mapper;
            ImageHelper = imageHelper;
        }

    }
}
