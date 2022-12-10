using Entities.Concrete;
using Shared.Entites.Abstract;

namespace Entities.Dtos
{
    public class ArticleListDto : DtoGetBase
    {
        public IList<Article> Articles { get; set; }
        //public ResultStatus resultStatus { get; set; } ortak oldugu icin dtobase sinfina yazildi
    }
}
