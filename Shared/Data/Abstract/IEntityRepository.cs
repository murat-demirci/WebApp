using Shared.Entites.Abstract;
using System.Linq.Expressions;

namespace Shared.Data.Abstract
{
    //ortak metotlar(Get,Set,Update,Delete) bulunur
    //soyut haldedirler somut halleride yazilmali
    //burdaki kodlar dopper, ado, entity.framework icin ortak kullanilabilir
    //Generic repositorydir
    public interface IEntityRepository<T> where T : class, IEntity, new()//generic, bu repositoryye type verilir ona gore islem yapilir
        //T class olmali,new veritabani nesneleri icin ve database icin IEntity
        //bu sayede buraya veri tabani nesneleri gelmesi sarti eklendi
    {
        //asenkron sekilde olacagi icin Task eklenir
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        //var kullanici = repository.GetAsync(k=>k.id==15) olmasi icin expression verilir
        //hangi kullaniciyi (veya baska bir seyi) istiyorsak ona predicatye(filtre) denir

        //gelen kullanicinin (veya baska bir seyi) yaninda baska seyler de getirmek icin
        //bir expression daha eklenir array olarak cunku yaninda birden fazla sey de istenebilir
        //birden fazla includeProperties olacagindan params eklenir
        //params: var makale = repository.Get(m=>m.id==25,m=>m.user,m=>comments) bunlar arraye eklenir ve parameteredir
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);
        //tum kategoriler | repository.GetAllAsync();
        //id 1 olan makale yorumlari repository.GetAllAsync(y=>y.ArticleId==1)
        //predicate null olabilir yani metot a deger verilmezse tum herseyi yukler

        //ekleme guncelleme islemleri icin entity donmeli
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        //any bir kullanici var mi? ayni makale varmi ? gibi sorgu yapmamizi saglar
        //var result = _userRepository.AnyAsync(u=>u.FirstName=="murat) murat isimli kullanici var mi?
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);

        Task<IList<T>> SearchAsync(IList<Expression<Func<T, bool>>> predicates, params Expression<Func<T, object>>[] includeProperties);

    }
}
