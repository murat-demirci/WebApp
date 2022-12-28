using AutoMapper;
using Entities.ComplexTypes;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mvc.Areas.Admin.Models;
using Mvc.Helpers.Abstract;
using NToastNotify;
using Shared.Utilities.Extensions;
using Shared.Utilities.Results.ComplexTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]

    //kullanici bazli yetkilendirme
    public class UserController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IToastNotification _toastNotification;

        public UserController(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IImageHelper imageHelper, IToastNotification toastNotification) : base(userManager, mapper, imageHelper)
        {
            _signInManager = signInManager;
            _toastNotification = toastNotification;
        }

        //authorize area kismina eklenirse sonsuz dongu olusur
        //tek tek actionlara eklenmeli
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await UserManager.Users.ToListAsync();
            return View(new UserListDto
            {
                Users = users,
                resultStatus = ResultStatus.Success
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<JsonResult> GetAllUsers()
        {
            var users = await UserManager.Users.ToListAsync();
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


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_UserAddPartial");
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            if (ModelState.IsValid)
            {
                var uploadedResult = await ImageHelper.Upload(userAddDto.UserName, userAddDto.PictureFile, PictureType.User);
                userAddDto.Picture = uploadedResult.ResultStatus == ResultStatus.Success
                    ? uploadedResult.Data.FullName
                    : "UserImages/defaultUser.jpg";
                //Imagehelper ile resim adi atama
                var user = Mapper.Map<User>(userAddDto);
                user.EmailConfirmed = true;
                var result = await UserManager.CreateAsync(user, userAddDto.Password);
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


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int userId)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            var result = await UserManager.DeleteAsync(user);
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


        [Authorize(Roles = "Admin,Editor")]
        [HttpGet]
        public async Task<PartialViewResult> Update(int userId)
        //partialviewresult da kullanilabilir (IActionResult yerine)
        {
            var user = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            //findbyidasync yerine firstordefaultasync de kullanilabilir
            var userUpdateDto = Mapper.Map<UserUpdateDto>(user);
            return PartialView("_UserUpdatePartial", userUpdateDto);
            //userupdatepartial viewi ve userupdatedto yu geri don
        }


        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isPictureUploaded = false;
                var oldUser = await UserManager.FindByIdAsync(userUpdateDto.Id.ToString());
                var oldPicture = oldUser.Picture;
                if (userUpdateDto.PictureFile != null)
                {
                    var uploadedResult = await ImageHelper.Upload(userUpdateDto.UserName, userUpdateDto.PictureFile, PictureType.User);
                    userUpdateDto.Picture = uploadedResult.ResultStatus == ResultStatus.Success
                        ? uploadedResult.Data.FullName
                        : "UserImages/defaultUser.jpg";
                    if (oldPicture != "UserImages/defaultUser.jpg")
                    {
                        isPictureUploaded = true;
                    }
                }
                var updatedUser = Mapper.Map(userUpdateDto, oldUser);
                var result = await UserManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    if (isPictureUploaded)
                    {
                        ImageHelper.Delete(oldPicture);
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

        [Authorize]
        [HttpGet]
        public async Task<ViewResult> ChangeDetail()
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            var updateDto = Mapper.Map<UserUpdateDto>(user);
            return View(updateDto);
        }


        [Authorize]
        [HttpPost]
        public async Task<ViewResult> ChangeDetail(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isPictureUploaded = false;
                var oldUser = await UserManager.GetUserAsync(HttpContext.User);
                var oldPicture = oldUser.Picture;
                if (userUpdateDto.PictureFile != null)
                {
                    var uploadedResult = await ImageHelper.Upload(userUpdateDto.UserName, userUpdateDto.PictureFile, PictureType.User);
                    userUpdateDto.Picture = uploadedResult.ResultStatus == ResultStatus.Success
                        ? uploadedResult.Data.FullName
                        : "UserImages/defaultUser.jpg";
                    if (oldPicture != "UserImages/defaultUser.jpg")
                    {
                        isPictureUploaded = true;
                    }

                }
                var updatedUser = Mapper.Map(userUpdateDto, oldUser);
                var result = await UserManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    if (isPictureUploaded)
                    {
                        ImageHelper.Delete(oldPicture);
                    }
                    _toastNotification.AddSuccessToastMessage($"{updatedUser.UserName} adlı kullanıcı başarıyla güncellenmiştir");
                    return View(userUpdateDto);
                }
                else
                {
                    //identity tarafinda olusacak hatalar icin
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                    return View(userUpdateDto);
                }
            }
            else
            {
                return View(userUpdateDto);
            }
        }

        [Authorize]
        [HttpGet]
        public ViewResult PasswordChange()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeDto passwordChangeDto)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUserAsync(HttpContext.User);
                var isVerified = await UserManager.CheckPasswordAsync(user, passwordChangeDto.CurrentPassword);
                if (isVerified)
                {
                    var result = await UserManager.ChangePasswordAsync(
                        user,
                        passwordChangeDto.CurrentPassword,
                        passwordChangeDto.NewPassword
                        );
                    if (result.Succeeded)
                    {
                        //securitystamp degeri degismeli
                        //verinin degistigini belirtir
                        await UserManager.UpdateSecurityStampAsync(user);
                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(
                            user,
                            passwordChangeDto.NewPassword,
                            true,//remember me degeri (7 gun)
                            false
                            );
                        _toastNotification.AddSuccessToastMessage("Şifreniz başarıyla değiştirilmiştir");
                        return View();
                    }
                    else
                    {
                        foreach (var err in result.Errors)
                        {
                            ModelState.AddModelError("", err.Description);
                        }
                        return View(passwordChangeDto);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Lutfen girmis oldugunuz bilgileri kontrol ediniz");
                    return View(passwordChangeDto);
                }
            }
            else
            {
                return View(passwordChangeDto);
            }
        }

        [HttpGet]
        public async Task<PartialViewResult> GetDetail(int userId)
        {
            var user = await UserManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            return PartialView("_GetDetailPartial", new UserDto { User = user });
        }
    }
}
