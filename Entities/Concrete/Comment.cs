using Shared.Entites.Abstract;

namespace Entities.Concrete
{
    public class Comment : EntityBase, IEntity
    {
        //eğer comment'in IsActive değerini AutoMapper'da değil'de burada false yapmak isteseydik
        //IsActive değerini override ederek false olarak atayabilirdik
        //public override bool IsActive { get; set; } = false;
        public string CommentContent { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
