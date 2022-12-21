using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "INSERT INTO dbo.Categories (Name,Note,CreatedDate,CreatedByName,ModifiedDate,ModifiedByName,IsActive,IsDeleted) VALUES ('Python','Python Kategorisi',GETDATE(),'Migration',GETDATE(),'Migration',1,0)");
            migrationBuilder.Sql(
                            "INSERT INTO dbo.Categories (Name,Note,CreatedDate,CreatedByName,ModifiedDate,ModifiedByName,IsActive,IsDeleted) VALUES ('Java','Java Kategorisi',GETDATE(),'Migration',GETDATE(),'Migration',1,0)");
            migrationBuilder.Sql(
                            "INSERT INTO dbo.Categories (Name,Note,CreatedDate,CreatedByName,ModifiedDate,ModifiedByName,IsActive,IsDeleted) VALUES ('Dart','Dart Kategorisi',GETDATE(),'Migration',GETDATE(),'Migration',1,0)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
