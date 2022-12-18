using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Concrete.EntityFramework.Mappings
{
    public class UserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> b)
        {
            // Primary key
            b.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            b.ToTable("AspNetUserRoles");


            b.HasData(
                new UserRole
                {
                    //admin icin atama
                    RoleId = 1,
                    UserId = 1,
                },
                new UserRole
                {
                    //editor icin atama
                    RoleId = 2,
                    UserId = 2,
                }
                );
        }
    }
}
