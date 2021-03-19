using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWorkShop.Identity.Introduction.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "E-Posta:")]
        [Required(ErrorMessage = "Email adresini giriniz.")]
        [EmailAddress(ErrorMessage = "E mail adresinizi doğru formatta giriniz")]
        public string Email { get; set; }

        [Display(Name = "Şifre:")]
        [Required(ErrorMessage = "Şifrenizi boş geçmeyiniz.")]
        [DataType(DataType.Password)]
        [MinLength(4,ErrorMessage = "En az 4 karakterli olmalıdır.")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
