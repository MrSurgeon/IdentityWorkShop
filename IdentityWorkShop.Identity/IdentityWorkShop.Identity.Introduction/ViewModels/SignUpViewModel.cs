using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWorkShop.Identity.Introduction.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Kullanıcı ismi gereklidir!")]
        [Display(Name = "Kullanıcı Adı:")]
        public string UserName { get; set; }

        [Display(Name = "Telefon No:")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "E-mail adresiniz gereklidir!")]
        [Display(Name = "E-Mail:")]
        [EmailAddress(ErrorMessage = "E-mail adresiniz doğru formatta değildir!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifrenizi boş geçemezsiniz!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
