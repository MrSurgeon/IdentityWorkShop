using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityWorkShop.Identity.Introduction.Models
{
    public class AppUser : IdentityUser
    {
        
        [StringLength(100, ErrorMessage = "100 karakterden fazla giriş şu an yapılamamaktadır.")]
        public string City { get; set; }

        public string Image { get; set; }
        public DateTime? BirthDay { get; set; }
        public int Gender { get; set; }

    }
}
