using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;

namespace Mvc.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAddDto, User>();
            //useradddto yu user sinifina donusturur
        }
    }
}
