using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Concrete.EntityFramework.Mappings
{
    public class UserClaimMap : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> b)
        {
            // Primary key
            b.HasKey(uc => uc.Id);

            // Maps to the AspNetUserClaims table
            b.ToTable("AspNetUserClaims");
        }
    }
}
