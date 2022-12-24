using AutoMapper;
using Entities.Dtos;
using Mvc.Areas.Admin.Models;

namespace Mvc.AutoMapper.Profiles
{
    public class ViewModelsProfile : Profile
    {
        public ViewModelsProfile()
        {
            CreateMap<ArticleAddViewModel, ArticleAddDto>();
        }
    }
}
