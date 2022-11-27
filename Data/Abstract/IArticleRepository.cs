using Entities.Concrete;
using Shared.Data.Abstract;
//veri tabani islemleri
namespace Data.Abstract
{
    //Entities ve shared katmanlari ekle
    //IEntitiyRepository generictir
    public interface IArticleRepository : IEntityRepository<Article>//IEntityRepository icindeki metotlar article icin calisir
    {
    }
}
