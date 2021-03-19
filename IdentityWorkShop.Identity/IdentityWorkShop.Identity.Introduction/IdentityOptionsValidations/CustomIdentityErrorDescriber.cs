using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IdentityWorkShop.Identity.Introduction.IdentityOptionsValidations
{
    public class CustomIdentityErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "InvalidUserName",
                Description = $"Bu {userName} geçersizdir. Lütfen Türkçe alfabesini kullanınız."
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = "InvalidUserName",
                Description = $"Bu {email} kullanılmaktadır. Lütfen farklı bir email adresi giriniz."
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return base.PasswordTooShort(length);
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "InvalidUserName",
                Description = $"Bu Kullanıcı Adı({userName}) kullanılmaktadır. Lütfen farklı bir username giriniz."
            };
        }
    }
}
