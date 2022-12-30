using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mvc.Areas.Admin.Models;
using Services.Abstract;
using Shared.Utilities.Results.ComplexTypes;

namespace Mvc.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ICategoryService _cateogryService;
        private readonly ICommentService _commentService;
        private readonly IArticleService _articleService;


        public HomeController(UserManager<User> userManager, ICategoryService cateogryService, ICommentService commentService, IArticleService articleService)
        {
            _userManager = userManager;
            _cateogryService = cateogryService;
            _commentService = commentService;
            _articleService = articleService;
        }

        [Authorize(Roles = "SuperAdmin,AdminArea.Home.Read")]
        public async Task<IActionResult> Index()
        {
            //tum iceriklerin sayisinin index saytfasina dinamik olarak getirilmesi
            var categoriesCount = await _cateogryService.CountByNonDeletedAsync();
            var articlesCount = await _articleService.CountByNonDeletedAsync();
            var commentCount = await _commentService.CountByNonDeletedAsync();
            var userCount = await _userManager.Users.CountAsync();
            var article = await _articleService.GetAllAsync();

            if (categoriesCount.ResultStatus == ResultStatus.Success && articlesCount.ResultStatus == ResultStatus.Success
                && commentCount.ResultStatus == ResultStatus.Success && userCount > -1 && article.ResultStatus == ResultStatus.Success)
            {
                return View(new DashboardViewModel
                {
                    CategoriesCount = categoriesCount.Data,
                    ArticlesCount = articlesCount.Data,
                    CommentsCount = commentCount.Data,
                    UsersCount = userCount,
                    Articles = article.Data
                });
            }
            return NotFound();
        }
    }
}
