using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mvc.Areas.Admin.Models;
using Mvc.Helpers.Abstract;
using Services.Abstract;
using Shared.Utilities.Extensions;
using Shared.Utilities.Results.ComplexTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : BaseController
    {
        /*index icinde kategorileri goruntulemek icin categoryservice cagrilir*/
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper) : base(userManager, mapper, imageHelper)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAllByNonDeletedAsync();
            //category service icinde dataresult dondugu icin result aliyoruz
            return View(result.Data);
            //data => categorylistdto dur
            //dtobase e message eklendi ve manageri a bu message eklendi bu sayede result i successs veya
            //error olma durumunu tek yerden aldik
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Category.Create")]
        public IActionResult Add()
        {
            return PartialView("_CategoryAddPartial");
        }



        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Category.Create")]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto)
        {
            //gelen bilgiler dolu mu  bos mu, dogru mu  kontrolu
            if (ModelState.IsValid)
            {
                var result = await _categoryService.AddAsync(categoryAddDto, LoggedInUser.UserName);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var ajaxAddCategory = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
                    {
                        CategoryDto = result.Data,
                        CategoryAddPartial = await this
                            .RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto)
                    });
                    return Json(ajaxAddCategory);//json formatinda doner
                }
            }
            var ajaxAddError = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
            {
                CategoryAddPartial = await this
                            .RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto)
                //partial icinde hatali inputlarla ilgili hata mesajlari doner
            });
            return Json(ajaxAddError);
        }


        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Category.Update")]
        public async Task<IActionResult> Update(int categoryId)
        {
            var result = await _categoryService.GetCategoryUpdateDtoAsync(categoryId);
            if (result.ResultStatus == ResultStatus.Success)
            {
                return PartialView("_CategoryUpdatePartial", result.Data);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Category.Update")]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto)
        {
            //gelen bilgiler dolu mu  bos mu, dogru mu  kontrolu
            if (ModelState.IsValid)
            {
                var result = await _categoryService.UpdateAsync(categoryUpdateDto, LoggedInUser.UserName);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var ajaxUpdateCategory = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel
                    {
                        CategoryDto = result.Data,
                        CategoryUpdatePartial = await this
                            .RenderViewToStringAsync("_CategoryUpdatePartial", categoryUpdateDto)
                    });
                    return Json(ajaxUpdateCategory);//json formatinda doner
                }
            }
            var ajaxUpdateError = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel
            {
                CategoryUpdatePartial = await this
                            .RenderViewToStringAsync("_CategoryUpdatePartial", categoryUpdateDto)
                //partial icinde hatali inputlarla ilgili hata mesajlari doner
            });
            return Json(ajaxUpdateError);
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        [HttpGet]
        public async Task<JsonResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllByNonDeletedAsync();
            var categories = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categories);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Category.Delete")]
        public async Task<JsonResult> Remove(int categoryId)
        {
            //index deki data-id den id alincak
            var result = await _categoryService.RemoveAsync(categoryId, LoggedInUser.UserName);
            var deletedCategory = JsonSerializer.Serialize(result.Data);
            return Json(deletedCategory);
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        [HttpGet]
        public async Task<IActionResult> DeletedCategories()
        {
            var result = await _categoryService.GetAllByDeletedAsync();
            return View(result.Data);
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        [HttpGet]
        public async Task<JsonResult> GetAllDeletedCategories()
        {
            var result = await _categoryService.GetAllByDeletedAsync();
            var categories = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categories);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Category.Update")]
        public async Task<JsonResult> UndoDelete(int categoryId)
        {
            //index deki data-id den id alincak
            var result = await _categoryService.UndoDeleteAsync(categoryId, LoggedInUser.UserName);
            var undoDeletedCategory = JsonSerializer.Serialize(result.Data);
            return Json(undoDeletedCategory);
        }

        //Kategoriyi veri tabanından silmek için HardDelete action'ına ihtiyaç var, ancak CategoryManager'da tanımolı olmaığı için eklemedim
        //[HttpPost]
        //[Authorize(Roles = "SuperAdmin,Category.Update")]
        //public async Task<JsonResult> HardDelete(int categoryId)
        //{
        //    //index deki data-id den id alincak
        //    var result = await _categoryService.HardDeleteAsync(categoryId);
        //    var deletedCategory = JsonSerializer.Serialize(result);
        //    return Json(deletedCategory);
        //}
    }
}
