using Data.Abstract;
using Data.Concrete;
using Data.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
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
            serviceCollection.AddDbContext<dContext>(
    options => options.UseSqlServer(
        "data source=LAPTOP-UI9DTME8;initial catalog=dboBlog;trusted_connection=true;Encrypt=False;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=True;"));
            //serviceCollection.AddDbContext<dContext>();
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
