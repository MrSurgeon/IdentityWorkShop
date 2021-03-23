using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWorkShop.Identity.Introduction.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Display(Name = "Yeni Şifre:")]
        [Required(ErrorMessage = "Şifrenizi boş geçmeyiniz.")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "En az 4 karakterli olmalıdır.")]
        public string Password { get; set; }

        [Display(Name = "Tekrar Yeni Şifre:")]
        [Required(ErrorMessage = "Şifrenizi boş geçmeyiniz.")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "En az 4 karakterli olmalıdır.")]
        [Compare(nameof(Password),ErrorMessage = "Şifreleriniz birbiriyle uyuşmamaktadır")]
        public string ConfirmedPassword { get; set; }

        public string Token { get; set; }
        public string Email { get; set; }
    }
}
