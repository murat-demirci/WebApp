using Shared.Entites.Abstract;

namespace Entities.Concrete
{
    //Entities
    //veri tabani nesneleri
    //veri tabaninda tablo olusturur 
    //dto veri tabani objelerinin parcalandigi ve gorev verdigi objelerdir
    //belirli bir alana eklme yapilmasi icin belirli verilerin gelmesini saglar (dto)

    //data katmanina:veritabani islemleri icin hem somut hem soyut olarak yazilmai bu kisimlar
    //exp. article ile ilgili islem yapilacaksa oncelikle IArticleRepository olusturulur
    //daha sonrada somut bir class ile icleri doldurulur
    //generic olmasi acisindan I....Repository leri IEntityRepository implemente edecek
    public class Category : EntityBase, IEntity//entitiy base parentm, IEntitiy implement(imza)
    {
        //concrete icine somut nesneler eklenir.
        //article manager gibi
        //data katmanindan erisilir

        //dependencies dan shared eklenir

        public string Name { get; set; }
        public ICollection<Article> Articles { get; set; }
        //ICollection i tum collectionlar implent eder
        //ICollection<int> list= new List<int>()
        ////burda List yerine Arraylist de yazilabilir ve sorun yasanmaz
    }
}
