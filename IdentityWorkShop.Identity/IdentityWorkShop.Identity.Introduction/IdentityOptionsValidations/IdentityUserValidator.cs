using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityWorkShop.Identity.Introduction.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityWorkShop.Identity.Introduction.IdentityOptionsValidations
{
    public class IdentityUserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors = new List<IdentityError>();
            var digits = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            foreach (var digit in digits)
            {
                if (user.UserName.StartsWith(digit))
                {
                    errors.Add(new IdentityError()
                    {
                        Code = "UserNameStartsWithIntCharacter",
                        Description = "Kullanıcı adı rakam ile başlayamaz"
                    });
                    break;
                }
            }
            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            else
            {
                return Task.FromResult(IdentityResult.Success);
            }
        }
    }
}
