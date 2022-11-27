using Shared.Entites.Abstract;

namespace Entities.Concrete
{
    public class Article : EntityBase, IEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ArticleThumbnail { get; set; }
        //public DateTime Date { get; set; }
        public int ArticleView { get; set; } = 0;
        public int ArticleLike { get; set; } = 0;
        public int ArticleComment { get; set; } = 0;
        public string SeoAuthor { get; set; }//tarayicilar icin aciklamalar
        public string SeoDescription { get; set; }
        public string SeoTags { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }//kategoriye ulasmak icin
        public int UserId { get; set; }
        public User User { get; set; }//kullaniciya ulasmak icin
        public ICollection<Comment> Comments { get; set; }

    }
}
