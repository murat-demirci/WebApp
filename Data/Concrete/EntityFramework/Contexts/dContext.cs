using Data.Concrete.EntityFramework.Mappings;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

//data katmanina entityframework,sqlserver ve design kurulur eklenir
//Maapings fluent api kullanarak veri tabanina gidecek nesnelerin ayarlarinin yapildigi yer

namespace Data.Concrete.EntityFramework.Contexts
{
    public class dContext : DbContext
    {
        //public dContext()
        //{

        //}
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //option builder kullanilarak connection stringi ekleme
        //    optionsBuilder.UseSqlServer(connectionString:
        //        "data source=LAPTOP-UI9DTME8;initial catalog=dboBlog;trusted_connection=true;Encrypt=False;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=True;");
        //}
        public dContext(DbContextOptions<dContext> option) : base(option)
        {

        }

        //mappingden sonraki kisim
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ArticleMap());
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new CommentMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            //veritabani olusturulurken buradaki konfigurasyon dosyalari uygulanacak

        }
    }
}
