using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Shared.Utilities.Results.ComplexTypes;

namespace Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _articleService.GetlAllByNoneDeletedAsync();
            if(result.ResultStatus == ResultStatus.Success)
            {
                return View(result.Data);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
    }
}
