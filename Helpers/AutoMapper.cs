using AutoMapper;
using dotnet8_introduction.Entities;
using dotnet8_introduction.Models;

namespace dotnet8_introduction.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateUserModel, User>();
        }
    }
}
