using AutoMapper;
using Entities.ComplexTypes;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using Mvc.Areas.Admin.Models;
using Mvc.Helpers.Abstract;
using Services.Abstract;
using Shared.Utilities.Results.ComplexTypes;

namespace Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IImageHelper _imageHelper;

        public ArticleController(IArticleService articleService, ICategoryService categoryService, IMapper mapper, IImageHelper imageHelper)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _mapper = mapper;
            _imageHelper = imageHelper;
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
        public async Task<IActionResult> Add()
        {
            var result = await _categoryService.GetAllByNonDeletedAsync();
            if (result.ResultStatus == ResultStatus.Success)
            {
                return View(new ArticleAddViewModel
                {
                    Categories = result.Data.Categories,
                });
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ArticleAddViewModel articleAddViewModel)
        {
            //eğer bu kod bloğu eklenmezse eklemeye çalıştığında kategoriye boş gördüğü için hata veriyor
            //ancak şu an herhangi bir hata vermemesine rağmen ekleme işlemini gerçekleştirmiyor
            var category = await _categoryService.GetAllByNonDeletedAsync();
            articleAddViewModel.Categories = category.Data.Categories;
            if (ModelState.IsValid)
            {
                var articleAddDto = _mapper.Map<ArticleAddDto>(articleAddViewModel);
                var imageResult = await _imageHelper.Upload(articleAddViewModel.Title, articleAddViewModel.ThumbnailFile, PictureType.Post);
                articleAddDto.ArticleThumbnail = imageResult.Data.FullName;
                //Base controller oluşturulduğu zaman createdbyname eklenecek (login olan kullanıcıdan alacak)
                var result = await _articleService.AddAsync(articleAddDto, "Geçici İsim");
                if(result.ResultStatus == ResultStatus.Success)
                {
                    TempData.Add("SuccessMessage", result.Message);
                    return RedirectToAction("Index", "Article");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    return View(articleAddViewModel);
                }
            }
            return View(articleAddViewModel);
        }
    }
}
