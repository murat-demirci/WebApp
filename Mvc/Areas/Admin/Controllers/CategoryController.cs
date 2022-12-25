﻿using AutoMapper;
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
    [Authorize(Roles = "Admin,Editor")]
    public class CategoryController : BaseController
    {
        /*index icinde kategorileri goruntulemek icin categoryservice cagrilir*/
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper) : base(userManager, mapper, imageHelper)
        {
            _categoryService = categoryService;
        }

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
        public IActionResult Add()
        {
            return PartialView("_CategoryAddPartial");
        }



        [HttpPost]
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
        public async Task<JsonResult> Remove(int categoryId)
        {
            //index deki data-id den id alincak
            var result = await _categoryService.RemoveAsync(categoryId, LoggedInUser.UserName);
            var deletedCategory = JsonSerializer.Serialize(result.Data);
            return Json(deletedCategory);
        }
    }
}
