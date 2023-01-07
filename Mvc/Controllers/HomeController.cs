using Microsoft.AspNetCore.Mvc;
using Services.Abstract;

namespace Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;

        public HomeController(IArticleService articleService)
        {
            _articleService = articleService;
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
    }
}
