using AutoMapper;
using My.Database.Models;
using My.App.Dtos;

namespace My.App.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
