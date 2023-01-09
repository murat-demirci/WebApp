using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.Abstract;

namespace Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly AboutUsPageInfo _aboutUsPageInfo;

        public HomeController(IArticleService articleService, IOptions<AboutUsPageInfo> aboutUsPageInfo)
        {
            _articleService = articleService;
            //ilgili section'ın içindeki değerleri okuyup atama yapar
            _aboutUsPageInfo = aboutUsPageInfo.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId, int currentPage = 1, int pageSize = 5)
        {
            //int? nullable old. için ve getallbycategoryid int kabul ettiği için categoryId'yi verirken categoryId.Value şeklinde verdik
            var articlesResult = await (categoryId == null 
                ? _articleService.GetAllByPagingAsync(null, currentPage, pageSize) 
                : _articleService.GetAllByPagingAsync(categoryId.Value, currentPage, pageSize));
            return View(articlesResult.Data);
        }

        [HttpGet]
        public IActionResult About()
        {
            return View(_aboutUsPageInfo);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(EmailSendDto emailSendDto)
        {
            return View();
        }
    }
}
