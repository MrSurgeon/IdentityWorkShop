using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using IdentityWorkShop.Identity.Introduction.Enums;
using Microsoft.AspNetCore.Mvc;
using IdentityWorkShop.Identity.Introduction.Models;
using IdentityWorkShop.Identity.Introduction.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IdentityWorkShop.Identity.Introduction.Controllers
{
    [Authorize]
    public class MemberController : BaseController
    {
        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper) : base(userManager, mapper, signInManager, null)
        {
        }
        // ben   Omer  
        public async Task<IActionResult> Index()
        {
            var user = await CurrentUser;

            var userViewModel = _mapper.Map<UserViewModel>(user);

            return View(userViewModel);
        }

        public IActionResult PasswordChange()
        {
            return View();
        }

        public async Task<IActionResult> UserEdit()
        {
            var user = await CurrentUser;
            var userEditModel = _mapper.Map<UserEditViewModel>(user);

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Genders)));
            return View(userEditModel);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await CurrentUser;

                var result = await _userManager.ChangePasswordAsync(user, passwordChangeViewModel.OldPassword,
                            passwordChangeViewModel.NewPassword);
                if (result.Succeeded)
                {
                    if ((await _userManager.UpdateSecurityStampAsync(user)).Succeeded)
                    {
                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(user, passwordChangeViewModel.NewPassword, true, false);
                        ViewBag.Success = "true";
                        return View();
                    }
                    else
                    {
                      AddModelErrors((await _userManager.UpdateSecurityStampAsync(user)).Errors);
                    }
                }
                else
                {
                    AddModelErrors(result.Errors);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel userEditViewModel, IFormFile userPicture)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Genders)));

                var user = await CurrentUser;

                var updateUser = _mapper.Map(userEditViewModel, user);


                if (userPicture != null && userPicture.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(userPicture.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await userPicture.CopyToAsync(stream);
                        updateUser.Image = "/img/" + fileName;
                    }
                }
                var result = await _userManager.UpdateAsync(updateUser);
                if (result.Succeeded)
                {
                    if ((await _userManager.UpdateSecurityStampAsync(user)).Succeeded)
                    {
                        await _signInManager.SignOutAsync();
                        await _signInManager.SignInAsync(user, true);
                        ViewBag.Success = "true";
                        return View();
                    }
                    ViewBag.Success = "true";
                }
                else
                {
                    AddModelErrors((await _userManager.UpdateSecurityStampAsync(user)).Errors);
                }
            }
            return View(userEditViewModel);
        }
    }
}
