using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityWorkShop.Identity.Introduction.Models;
using IdentityWorkShop.Identity.Introduction.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace IdentityWorkShop.Identity.Introduction.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }

       
    }
}
