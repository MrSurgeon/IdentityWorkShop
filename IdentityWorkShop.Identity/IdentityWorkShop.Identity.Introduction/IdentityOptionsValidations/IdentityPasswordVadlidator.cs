using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityWorkShop.Identity.Introduction.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityWorkShop.Identity.Introduction.IdentityOptionsValidations
{
    public class IdentityPasswordVadlidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            var errors = new List<IdentityError>();
            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                if (!(user.Email.Contains(user.UserName)))
                {
                    errors.Add(new IdentityError()
                    {
                        Code = "PasswordContainsUserName",
                        Description = "Şifre Kullanıcı Adı İçeremez."
                    });
                }
            }

            if (password.Contains("1234"))
            {
                errors.Add(new IdentityError()
                {
                    Code = "PasswordContains1234",
                    Description = "Şifre alanı 1234 karakterlerini ardışık bir şekilde içeremez."
                });
            }

            if (password.ToLower().Contains(user.Email.ToLower()))
            {
                errors.Add(new IdentityError()
                {
                    Code = "PasswordContainsEmail",
                    Description = "Şifre alanı emailinizi içeremez."
                });
            }

            if (!errors.Any())
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
        }
    }
}
