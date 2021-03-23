using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityWorkShop.Identity.Introduction.Models;
using IdentityWorkShop.Identity.Introduction.ViewModels;

namespace IdentityWorkShop.Identity.Introduction.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SignUpViewModel,AppUser>();
            CreateMap<LoginViewModel, AppUser>();
            CreateMap<UserViewModel, AppUser>();
            CreateMap<AppUser,UserViewModel>();
            CreateMap<AppUser, UserEditViewModel>();
            CreateMap<UserEditViewModel, AppUser>();
        }
    }
}
