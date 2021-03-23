using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWorkShop.Identity.Introduction.ViewModels
{
    public class PasswordChangeViewModel
    {
        [Required(ErrorMessage = "Eski şifreniz gereklidir.")]
        [Display(Name = "Eski Şifre:")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "En az 4 karakterli olmalıdır.")]
        public string OldPassword { get; set; }

        [Display(Name = "Yeni Şifre:")]
        [Required(ErrorMessage = "Şifrenizi boş geçmeyiniz.")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "En az 4 karakterli olmalıdır.")]
        public string NewPassword { get; set; }

        [Display(Name = "Tekrar Yeni Şifre:")]
        [Required(ErrorMessage = "Yeni şifrenizi tekrar girmelisiniz.")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "En az 4 karakterli olmalıdır.")]
        [Compare(nameof(NewPassword), ErrorMessage = "Şifreleriniz birbiriyle uyuşmamaktadır")]
        public string NewConfirmedPassword { get; set; }

    }
}
