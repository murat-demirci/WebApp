using Data.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Concrete.Dapper;

namespace Data.Concrete.EntityFramework.Repositories
{
    //bu kisim olusturulan soyut nesnenin somut hali
    //ici bos olan metotlarin doldurulmasi

    //efEntityRepository den turer, icindeki tum metotlari alir, calisacagi tip verilir
    //constructor icine db context verilmeli ve base e gonderilmeli
    //tek bir base sinif ile tum metotlar eklenir
    //ekstra metot eklenebilir bunun icin once Interface e yaz metotdu sonra burda implemente et icini doldur
    public class efArticleRepository : efEntityRepositoryBase<Article>, IArticleRepository
    {
        public efArticleRepository(DbContext context) : base(context)
        {

        }
    }
}
