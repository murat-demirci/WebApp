using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Concrete.EntityFramework.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> b)
        {
            b.Property(u => u.UserPicture).IsRequired();
            b.Property(u => u.UserPicture).HasMaxLength(250);

            // tum maplerden sonra db context icerisinde cagirmaya dcontexte
            // Primary key
            b.HasKey(u => u.Id);

            // Indexes for "normalized" username and email, to allow efficient lookups
            b.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
            b.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");

            // Maps to the AspNetUsers table
            b.ToTable("AspNetUsers");

            // A concurrency token for use with the optimistic concurrency checking
            b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            // Limit the size of columns to use efficient database types
            b.Property(u => u.UserName).HasMaxLength(50);
            b.Property(u => u.NormalizedUserName).HasMaxLength(50);
            b.Property(u => u.Email).HasMaxLength(100);
            b.Property(u => u.NormalizedEmail).HasMaxLength(100);

            // The relationships between User and other entity types
            // Note that these relationships are configured with no navigation properties

            // Each User can have many UserClaims
            b.HasMany<UserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

            // Each User can have many UserLogins
            b.HasMany<UserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

            // Each User can have many UserTokens
            b.HasMany<UserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

            // Each User can have many entries in the UserRole join table
            b.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();


            //fluent api ile ilk kullaniclari ekleme
            var adminUser = new User()
            {
                Id = 1,
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@mail.com",
                NormalizedEmail = "ADMIN@MAIL.COM",
                EmailConfirmed = true,
                UserPicture = "Default/defaultUser.jpg",
                SecurityStamp = Guid.NewGuid().ToString(),
                //bu deger guid degeri, arada - yok, - icin tostring("D") yaz

            };
            adminUser.PasswordHash = CreatePasswordHash(adminUser, "Admin1!");

            var editorUser = new User()
            {
                Id = 2,
                UserName = "Editor",
                NormalizedUserName = "EDITOR",
                Email = "editor@mail.com",
                NormalizedEmail = "EDITOR@MAIL.COM",
                EmailConfirmed = true,
                UserPicture = "Default/defaultUser.jpg",
                SecurityStamp = Guid.NewGuid().ToString(),
                //bu deger guid degeri, arada - yok, - icin tostring("D") yaz

            };
            editorUser.PasswordHash = CreatePasswordHash(editorUser, "Editor1!");

            b.HasData(adminUser, editorUser);
        }

        //rol tablosu degerleri ekle, sonra userrole tablosundan coka cok iliski uzerinden ekle

        //parola hasleme
        private string CreatePasswordHash(User user, string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            return passwordHasher.HashPassword(user, password);
        }
    }
}
