using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mvc.Areas.Admin.Models;
using Shared.Utilities.Extensions;
using Shared.Utilities.Results.ComplexTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;
        //wwroot dosya yolunu dinakilestirmek icin
        //farkli isletim sistemlerinde dosya yolu ayni kalir
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, IWebHostEnvironment env, IMapper mapper)
        {
            _userManager = userManager;
            _env = env;
            _mapper = mapper;
        }
        [Authorize]
        //authorize area kismina eklenirse sonsuz dongu olusur
        //tek tek actionlara eklenmeli

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(new UserListDto
            {
                Users = users,
                resultStatus = ResultStatus.Success
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<JsonResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userListDto = JsonSerializer.Serialize(new UserListDto
            {
                Users = users,
                resultStatus = ResultStatus.Success
            }, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(userListDto);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_UserAddPartial");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            if (ModelState.IsValid)
            {
                userAddDto.UserPicture = await ImageUpload(userAddDto.UserName, userAddDto.UserPictureFile);
                //resim adi atama
                var user = _mapper.Map<User>(userAddDto);
                var result = await _userManager.CreateAsync(user, userAddDto.Password);
                //kullanici ekleme
                if (result.Succeeded)//identityresult doner
                {
                    var userAddAjax = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            resultStatus = ResultStatus.Success,
                            Message = $"{user.UserName} adli kullanici eklenmistir",
                            User = user
                        },
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(userAddAjax);
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                    var userAddAjaxError = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserAddDto = userAddDto,
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(userAddAjaxError);
                    //identity tarafindaki hatayi frtonende gosterme
                }
            }
            var userAddAjaxErrorState = JsonSerializer.Serialize(new UserAddAjaxViewModel
            {
                UserAddDto = userAddDto,
                UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
            });
            return Json(userAddAjaxErrorState);
        }

        [Authorize]
        public async Task<IActionResult> Remove(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                var deletedUser = JsonSerializer.Serialize(new UserDto
                {
                    resultStatus = ResultStatus.Success,
                    Message = $"{user.UserName} adli kullanici silinmistir",
                    User = user
                });
                return Json(deletedUser);
            }
            else
            {
                string errMessages = string.Empty;
                foreach (var err in result.Errors)
                {
                    errMessages = $"*{err.Description}\n";
                }
                var deletedUserErrorModel = JsonSerializer.Serialize(new UserDto
                {
                    resultStatus = ResultStatus.Error,
                    Message = $"{user.UserName} silinirken bazi hatalar olustu\n{errMessages}",
                    User = user
                });
                return Json(deletedUserErrorModel);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<PartialViewResult> Update(int userId)
        //partialviewresult da kullanilabilir (IActionResult yerine)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            //findbyidasync yerine firstordefaultasync de kullanilabilir
            var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
            return PartialView("_UserUpdatePartial", userUpdateDto);
            //userupdatepartial viewi ve userupdatedto yu geri don
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isPictureUploaded = false;
                var oldUser = await _userManager.FindByIdAsync(userUpdateDto.Id.ToString());
                var oldPicture = oldUser.UserPicture;
                if (userUpdateDto.UserPictureFile != null)
                {
                    userUpdateDto.UserPicture =
                        await ImageUpload(userUpdateDto.UserName, userUpdateDto.UserPictureFile);
                    isPictureUploaded = true;
                }
                var updatedUser = _mapper.Map(userUpdateDto, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    if (isPictureUploaded)
                    {
                        ImageDelete(oldPicture);
                    }
                    var userUpdateAjax = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            resultStatus = ResultStatus.Success,
                            Message = $"{updatedUser.UserName} basariyla guncellenmistir.",
                            User = updatedUser
                        },
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                    });
                    return Json(userUpdateAjax);
                }
                else
                {
                    //identity tarafinda olusacak hatalar icin
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                    var userUpdateErrAjax = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserUpdateDto = userUpdateDto,
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                    });
                    return Json(userUpdateErrAjax);
                }
            }
            else
            {
                var userUpdateErrModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                {
                    UserUpdateDto = userUpdateDto,
                    UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                });
                return Json(userUpdateErrModel);
            }
        }

        public async Task<string> ImageUpload(string userName, IFormFile pictureFile)
        {
            string wwwroot = _env.WebRootPath;
            //wwwrootun dosya yolunu dinamik olarak verir
            //string fileNameOrg = Path.GetFileNameWithoutExtension(userAddDto.UserPictureFile.FileName);
            //resim dosyasinin sonundaki eklentiyi almaz sadece dosya adi gelir\ ornek amacli yazildi
            string fileExtension = Path.GetExtension(pictureFile.FileName);
            //dosya sonundaki format alinir
            string fileName = $"{ImageExtensions.CreateGuid()}_{userName}{fileExtension}";
            //dosya adi olusturulud
            var path = Path.Combine($"{wwwroot}/img", fileName);
            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
                //resim img klasorune aktarilir
                return fileName.ToString();
            }
        }

        public bool ImageDelete(string pictureName)
        {
            //guncelleme isleminde eski resmi silmek icin kullanilacak
            string wwwroot = _env.WebRootPath;
            var path = Path.Combine($"{wwwroot}/img", pictureName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
