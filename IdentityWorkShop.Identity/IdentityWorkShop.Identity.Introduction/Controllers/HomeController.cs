using IdentityWorkShop.Identity.Introduction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityWorkShop.Identity.Introduction.Models;
using IdentityWorkShop.Identity.Introduction.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace IdentityWorkShop.Identity.Introduction.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;

        public HomeController(UserManager<AppUser> userManager, IMapper mapper, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string ReturnUrl)
        {
            TempData["returnUrl"] = ReturnUrl;

            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<AppUser>(signUpViewModel);
                var result = await _userManager.CreateAsync(user, signUpViewModel.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(signUpViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, true);
                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        if (TempData["returnUrl"] != null)
                        {
                            return Redirect(TempData["returnUrl"].ToString());
                        }

                        return RedirectToAction("Index", "Member");
                    }
                    //İkinci yol olarak userManager.IsLockedAsync methodu da kullanılabilir.
                    else if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty,"Hesabınız kilitlenmiştir.");
                        return View(loginViewModel);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Başarısız şifre denemesi");
                        return View(loginViewModel);
                    }
                }
                ModelState.AddModelError(string.Empty,"Geçersiz Email Adresi veya Şifresi");
            }
            return View(loginViewModel);
        }
    }
}
