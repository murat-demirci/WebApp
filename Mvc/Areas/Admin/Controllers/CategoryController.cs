﻿using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using Mvc.Areas.Admin.Models;
using Services.Abstract;
using Shared.Utilities.Extensions;
using Shared.Utilities.Results.ComplexTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        /*index icinde kategorileri goruntulemek icin categoryservice cagrilir*/
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAll();
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
                var result = await _categoryService.Add(categoryAddDto, "Admin");
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

        public async Task<JsonResult> GetAllCategories()
        {
            var result = await _categoryService.GetAll();
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
            var result = await _categoryService.Remove(categoryId, "Admin");
            var ajaxResult = JsonSerializer.Serialize(result);
            return Json(ajaxResult);
        }
    }
}
