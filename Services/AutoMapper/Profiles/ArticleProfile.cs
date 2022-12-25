using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;

namespace Services.AutoMapper.Profiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            //CreateMap generic,ilk kaynagimiz yani neyi neye donusturecegimizi veriyoruz

            CreateMap<ArticleAddDto, Article>().ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(x => DateTime.Now));
            //articleadddto icinde createddate yok burada ayarlamasi yapiliyor
            CreateMap<ArticleUpdateDto, Article>().ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<Article, ArticleUpdateDto>();
        }
    }
}
