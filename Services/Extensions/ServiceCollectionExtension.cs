using Data.Abstract;
using Data.Concrete;
using Data.Concrete.EntityFramework.Contexts;
using Entities.Concrete;

using Microsoft.Extensions.DependencyInjection;
using Services.Abstract;
using Services.Concrete;

namespace Services.Extensions
{
    //extensionlar mvc ve data katmaninin arasinda bulunur
    //katmanlar kendilerinden bir ust katmana ersimelidir
    //bu sebeble data katmani dogrudan mvc katmanina erismesi dogru degildir
    //service katamni bu iki katman arasinda kopru gorevi gorur
    //static olurlar
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<dContext>();
            serviceCollection.AddIdentity<User, Role>().AddEntityFrameworkStores<dContext>();
            //migrations islemleri icin(identity)
            serviceCollection.AddScoped<IUnitofWork, UnitofWork>();
            //scoped: bir istekte bulundugunda ve bu islemlere baslandiginda bu islemlerin
            //butunu scope icerisine alinir ve yurutulur.
            //tum islemler bittikten sonra (site ile baglanti kesildiignde) scope da kendini kapatir
            serviceCollection.AddScoped<IArticleService, ArticleManager>();
            serviceCollection.AddScoped<ICategoryService, CategoryManager>();


            return serviceCollection;
            //burdan startup.cs dosyasina git
        }

    }
}
