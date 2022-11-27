using Entities.Concrete;
using Shared.Entites.Abstract;

namespace Entities.Dtos.Articles
{
    public class ArticleDto : DtoGetBase
    {
        public Article Article { get; set; }
        //public ResultStatus resultStatus { get; set; }
        //public override ResultStatus resultStatus { get; set; } = ResultStatus.Success;
        //ornek kullanim
    }
}
