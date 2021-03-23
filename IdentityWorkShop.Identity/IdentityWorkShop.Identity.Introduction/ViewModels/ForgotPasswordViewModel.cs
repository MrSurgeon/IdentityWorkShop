using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWorkShop.Identity.Introduction.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Display(Name = "E-Posta:")]
        [Required(ErrorMessage = "Email adresini giriniz.")]
        [EmailAddress(ErrorMessage = "E mail adresinizi doğru formatta giriniz")]
        public string Email { get; set; }
    }
}
