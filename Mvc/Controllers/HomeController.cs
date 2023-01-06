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
        public async Task<IActionResult> Index(int? categoryId)
        {
            //int? nullable old. için ve getallbycategoryid int kabul ettiği için categoryId'yi verirken categoryId.Value şeklinde verdik
            var articlesResult = await (categoryId == null ? _articleService.GetAllByNonDeletedAndActiveAsync() : _articleService.GetAllByCategoryAsync(categoryId.Value));
            return View(articlesResult.Data);
        }
    }
}
