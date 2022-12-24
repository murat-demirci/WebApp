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
using Shared.Utilities.Extensions;
using Shared.Utilities.Results.ComplexTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]

    //kullanici bazli yetkilendirme
    public class UserController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IImageHelper _imageHelper;

        public UserController(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IImageHelper imageHelper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _imageHelper = imageHelper;
        }

        //authorize area kismina eklenirse sonsuz dongu olusur
        //tek tek actionlara eklenmeli
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
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
                var uploadedResult = await _imageHelper.Upload(userAddDto.UserName, userAddDto.UserPictureFile, PictureType.User);
                userAddDto.UserPicture = uploadedResult.ResultStatus == ResultStatus.Success
                    ? uploadedResult.Data.FullName
                    : "UserImages/defaultUser.jpg";
                //Imagehelper ile resim adi atama
                var user = _mapper.Map<User>(userAddDto);
                user.EmailConfirmed = true;
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


        [Authorize(Roles = "Admin")]
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


        [Authorize(Roles = "Admin,Editor")]
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


        [Authorize(Roles = "Admin,Editor")]
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
                    var uploadedResult = await _imageHelper.Upload(userUpdateDto.UserName, userUpdateDto.UserPictureFile, PictureType.User);
                    userUpdateDto.UserPicture = uploadedResult.ResultStatus == ResultStatus.Success
                        ? uploadedResult.Data.FullName
                        : "UserImages/defaultUser.jpg";
                    if (oldPicture != "UserImages/defaultUser.jpg")
                    {
                        isPictureUploaded = true;
                    }
                }
                var updatedUser = _mapper.Map(userUpdateDto, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    if (isPictureUploaded)
                    {
                        _imageHelper.Delete(oldPicture);
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
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var updateDto = _mapper.Map<UserUpdateDto>(user);
            return View(updateDto);
        }


        [Authorize]
        [HttpPost]
        public async Task<ViewResult> ChangeDetail(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isPictureUploaded = false;
                var oldUser = await _userManager.GetUserAsync(HttpContext.User);
                var oldPicture = oldUser.UserPicture;
                if (userUpdateDto.UserPictureFile != null)
                {
                    var uploadedResult = await _imageHelper.Upload(userUpdateDto.UserName, userUpdateDto.UserPictureFile, PictureType.User);
                    userUpdateDto.UserPicture = uploadedResult.ResultStatus == ResultStatus.Success
                        ? uploadedResult.Data.FullName
                        : "UserImages/defaultUser.jpg";
                    if (oldPicture != "UserImages/defaultUser.jpg")
                    {
                        isPictureUploaded = true;
                    }

                }
                var updatedUser = _mapper.Map(userUpdateDto, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    if (isPictureUploaded)
                    {
                        _imageHelper.Delete(oldPicture);
                    }
                    TempData.Add("SuccessMessage", $"{updatedUser.UserName} basariyla guncellenmistir.");
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
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var isVerified = await _userManager.CheckPasswordAsync(user, passwordChangeDto.CurrentPassword);
                if (isVerified)
                {
                    var result = await _userManager.ChangePasswordAsync(
                        user,
                        passwordChangeDto.CurrentPassword,
                        passwordChangeDto.NewPassword
                        );
                    if (result.Succeeded)
                    {
                        //securitystamp degeri degismeli
                        //verinin degistigini belirtir
                        await _userManager.UpdateSecurityStampAsync(user);
                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(
                            user,
                            passwordChangeDto.NewPassword,
                            true,//remember me degeri (7 gun)
                            false
                            );
                        TempData.Add("SuccessMessage", $"Sifreniz basariyla degistirilmistir");
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
    }
}
