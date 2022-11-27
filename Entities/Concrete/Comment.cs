using Shared.Entites.Abstract;

namespace Entities.Concrete
{
    public class Comment : EntityBase, IEntity
    {
        public string CommentContent { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
