using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using EmailService;
using IdentityWorkShop.Identity.Introduction.Models;
using IdentityWorkShop.Identity.Introduction.ViewModels;
using Microsoft.AspNetCore.Identity;


namespace IdentityWorkShop.Identity.Introduction.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<AppUser> userManager, IMapper mapper,
            SignInManager<AppUser> signInManager, IEmailSender emailSender):base(userManager, mapper, signInManager, emailSender)
        {
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Member");
            }
            return View();
        }

        public IActionResult Login(string ReturnUrl)
        {
            if (@User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index), "Member");
            }
            else
            {
                TempData["returnUrl"] = ReturnUrl;

                return View();
            }

        }
        public IActionResult SignUp()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult ForgotPasswordInformation()
        {
            return View();
        }
        public IActionResult ResetPasswordInformation()
        {
            return View();
        }

        public IActionResult ResetPassword(string email, string token)
        {
            var model = new ResetPasswordViewModel() { Token = token, Email = email };
            return View(model);
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
                   AddModelErrors(result.Errors);
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
                        ModelState.AddModelError(string.Empty, "Hesabınız kilitlenmiştir.");
                        return View(loginViewModel);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Başarısız şifre denemesi");
                        return View(loginViewModel);
                    }
                }
                ModelState.AddModelError(string.Empty, "Geçersiz Email Adresi veya Şifresi");
            }
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (!ModelState.IsValid)
                return View(forgotPasswordViewModel);
            var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
            if (user != null)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callback = Url.Action(nameof(ResetPassword), "Home", new { token, email = user.Email },
                    Request.Scheme);
                var message = new Message(new string[] { user.Email }, "Reset Password Token", callback);
                await _emailSender.SendEmailAsync(message);
                return RedirectToAction(nameof(ForgotPasswordInformation));

            }
            ModelState.AddModelError("", "Girdiğiniz bilgilere ait kullanıcı bulunamadı.");
            return View(forgotPasswordViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordViewModel);
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (user == null)
            {
                return RedirectToAction(nameof(ResetPasswordInformation));
            }

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Token,
                resetPasswordViewModel.Password);
            if (!resetPassResult.Succeeded)
            {
                AddModelErrors(resetPassResult.Errors);

                return View(resetPasswordViewModel);
            }

            return RedirectToAction(nameof(ResetPasswordInformation));
        }

    }
}
