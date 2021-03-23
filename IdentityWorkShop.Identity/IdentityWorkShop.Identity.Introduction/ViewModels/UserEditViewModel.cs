using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IdentityWorkShop.Identity.Introduction.Enums;

namespace IdentityWorkShop.Identity.Introduction.ViewModels
{
    public class UserEditViewModel
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
        [Display(Name = "Şehir:")]
        public string City { get; set; }

        [Display(Name = "Profil Resmi:")]
        //[DataType(DataType.ImageUrl,ErrorMessage = "Lütfen geçerli bir dosya yolu giriniz.")]
        public string Image { get; set; }

        [Display(Name = "Doğum Tarihi:")]
        [DataType(DataType.DateTime)]
        public DateTime? BirthDay { get; set; }

        [Display(Name = "Cinsiyet:")]
        public Genders Gender { get; set; }

    }
}
