using Entities.Dtos;

namespace Mvc.Models
{
    public class ArticleSearchViewModel
    {
        public ArticleListDto ArticleListDto { get; set; }
        public string Keyword { get; set; }

    }
}
