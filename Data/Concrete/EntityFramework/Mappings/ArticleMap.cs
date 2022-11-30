using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Concrete.EntityFramework.Mappings
{
    //data base e giderken nesnelerin sartlandirilmasi
    //exp. id primary key olacak mi , alan null olabilir mi, max uzunluk...vs
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(a => a.ID);//primarykey
            builder.Property(a => a.ID).ValueGeneratedOnAdd();//eklendikce arttir
            builder.Property(a => a.Title).HasMaxLength(100);
            builder.Property(a => a.Title).IsRequired();
            builder.Property(a => a.Content).IsRequired();
            builder.Property(a => a.ArticleThumbnail).IsRequired();
            builder.Property(a => a.ArticleThumbnail).HasMaxLength(250);
            builder.Property(a => a.Content).HasColumnType("NVARCHAR(MAX)");//ne kadar alabiliyorsa o kadar alir
            builder.Property(a => a.SeoAuthor).IsRequired();
            builder.Property(a => a.SeoAuthor).HasMaxLength(50);
            builder.Property(a => a.SeoDescription).IsRequired();
            builder.Property(a => a.SeoDescription).HasMaxLength(150);
            builder.Property(a => a.SeoTags).IsRequired();
            builder.Property(a => a.SeoTags).HasMaxLength(70);
            builder.Property(a => a.ArticleView).IsRequired();
            builder.Property(a => a.ArticleComment).IsRequired();
            builder.Property(a => a.ArticleLike).IsRequired();
            builder.Property(a => a.CreatedByName).IsRequired();
            builder.Property(a => a.CreatedByName).HasMaxLength(50);
            builder.Property(a => a.ModifiedByName).IsRequired();
            builder.Property(a => a.ModifiedByName).HasMaxLength(50);
            builder.Property(a => a.CreatedDate).IsRequired();
            builder.Property(a => a.ModifiedDate).IsRequired();
            builder.Property(a => a.IsActive).IsRequired();
            builder.Property(a => a.IsDeleted).IsRequired();
            builder.Property(a => a.Note).HasMaxLength(500);
            builder.HasOne<Category>(a => a.Category).WithMany(c => c.Articles).HasForeignKey(a => a.CategoryId);
            builder.HasOne<User>(a => a.User).WithMany(u => u.Articles).HasForeignKey(a => a.UserId);//1e cok iliski
            //bir makale olusturulurken bir kategoriye ve bir kullaniciya ihtiyac oldugu belirtildi
            builder.ToTable("Articles");//tabloya donusturme

            //builder.HasData(new Article
            //{
            //    ID = 1,
            //    UserId = 1,
            //    CategoryId = 1,
            //    Title = "Javascript ile animasyon",
            //    Content = "JavaScript Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of 'de Finibus Bonorum et Malorum' (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum",
            //    ArticleThumbnail = "article.jpg",
            //    ArticleComment = 1,
            //    ArticleLike = 0,
            //    ArticleView = 0,
            //    SeoDescription = "Javascript ile animasyon",
            //    SeoTags = "javscript",
            //    SeoAuthor = "Creator Name",
            //    IsActive = true,
            //    IsDeleted = false,
            //    CreatedByName = "InitialCreate",
            //    ModifiedByName = "InitialCreate",
            //    CreatedDate = DateTime.Now,
            //    ModifiedDate = DateTime.Now,
            //    Note = ""
            //});
        }
    }
}
