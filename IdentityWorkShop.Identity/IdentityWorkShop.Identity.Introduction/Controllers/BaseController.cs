using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EmailService;
using IdentityWorkShop.Identity.Introduction.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace IdentityWorkShop.Identity.Introduction.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserManager<AppUser> _userManager;
        protected readonly SignInManager<AppUser> _signInManager;
        protected readonly IMapper _mapper;
        protected readonly IEmailSender _emailSender;

        protected Task<AppUser> CurrentUser =>  _userManager.FindByNameAsync(User.Identity.Name);

        
        public BaseController(UserManager<AppUser> userManager, IMapper mapper, SignInManager<AppUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public void AddModelErrors(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty,error.Description);
            }
        }

    }
}
