using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Picture = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    About = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    YoutubeLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TwitterLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    InstagramLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FacebookLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    LinkedInLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    GitHubLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    WebsiteLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    ArticleThumbnail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ArticleView = table.Column<int>(type: "int", nullable: false),
                    ArticleLike = table.Column<int>(type: "int", nullable: false),
                    ArticleComment = table.Column<int>(type: "int", nullable: false),
                    SeoAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SeoDescription = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SeoTags = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentContent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comments_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ID", "CreatedByName", "CreatedDate", "IsActive", "IsDeleted", "ModifiedByName", "ModifiedDate", "Name", "Note" },
                values: new object[,]
                {
                    { 1, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5552), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5556), "C#", "C# Blog Kategorisi" },
                    { 2, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5570), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5573), "C++", "C++ Blog Kategorisi" },
                    { 3, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5584), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5587), "JavaScript", "JavaScript Blog Kategorisi" },
                    { 4, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5597), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5600), "Typescript", "Typescript Blog Kategorisi" },
                    { 5, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5611), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5614), "Java", "Java Blog Kategorisi" },
                    { 6, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5624), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5627), "Python", "Python Blog Kategorisi" },
                    { 7, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5637), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5641), "Php", "Php Blog Kategorisi" },
                    { 8, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5652), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5654), "Kotlin", "Kotlin Blog Kategorisi" },
                    { 9, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5665), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5668), "Swift", "Swift Blog Kategorisi" },
                    { 10, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5679), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 336, DateTimeKind.Local).AddTicks(5682), "Ruby", "Ruby Blog Kategorisi" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "076f61b9-dbf7-4139-aa9c-b4b07cbedd56", "Category.Create", "CATEGORY.CREATE" },
                    { 2, "31cdeef9-6621-4fcb-95fc-5c869090f2b7", "Category.Read", "CATEGORY.READ" },
                    { 3, "abc97f81-44f8-47d8-95db-f4eaca787f5c", "Category.Update", "CATEGORY.UPDATE" },
                    { 4, "1860f823-ba5f-426d-9e5a-6dcef241bb03", "Category.Delete", "CATEGORY.DELETE" },
                    { 5, "78a2eae2-8d2f-45c7-a96e-a7a1ccdbe9a6", "Article.Create", "ARTICLE.CREATE" },
                    { 6, "79fa8ce9-623f-4728-913c-92896e66af2c", "Article.Read", "ARTICLE.READ" },
                    { 7, "70045dcf-10c3-4d16-83e3-698a308813aa", "Article.Update", "ARTICLE.UPDATE" },
                    { 8, "f4a71ff2-eb8c-40f1-a3eb-8cc89cad9807", "Article.Delete", "ARTICLE.DELETE" },
                    { 9, "52f6024e-bb74-4bbe-8a18-21cb25e58725", "User.Create", "USER.CREATE" },
                    { 10, "bb82d319-a042-4104-9bd6-4b1d47a3bc3a", "User.Read", "USER.READ" },
                    { 11, "2ee568f7-e302-4251-9680-4b9c162e0083", "User.Update", "USER.UPDATE" },
                    { 12, "4a3174ae-f122-4a73-9d0a-0140c8dd553c", "User.Delete", "USER.DELETE" },
                    { 13, "f7bce70f-af2d-45fd-add8-0e0e7379e8f8", "Role.Create", "ROLE.CREATE" },
                    { 14, "37109d9e-9fef-47d6-b8de-8fe1db599469", "Role.Read", "ROLE.READ" },
                    { 15, "2d08edf3-f5ec-4609-aa0a-f7703723bc92", "Role.Update", "ROLE.UPDATE" },
                    { 16, "075a13b9-11fe-4d5a-9861-3f156ebf199b", "Role.Delete", "ROLE.DELETE" },
                    { 17, "b82bf500-c6db-48f4-abfc-5b8fb2a6580d", "Comment.Create", "COMMENT.CREATE" },
                    { 18, "de7c1d62-13b0-459a-9474-6a540a4cc2f9", "Comment.Read", "COMMENT.READ" },
                    { 19, "c7a4f859-96b6-44f4-bc68-da332f38ce01", "Comment.Update", "COMMENT.UPDATE" },
                    { 20, "d0a7d375-59b5-4e11-8834-4945e0b6f3b4", "Comment.Delete", "COMMENT.DELETE" },
                    { 21, "dd6033b5-a455-4460-9936-679e530fb318", "AdminArea.Home.Read", "ADMINAREA.HOME.READ" },
                    { 22, "1959a6a0-057a-4cb3-9a99-4caf96506115", "SuperAdmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "About", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FacebookLink", "FirstName", "GitHubLink", "InstagramLink", "LastName", "LinkedInLink", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Picture", "SecurityStamp", "TwitterLink", "TwoFactorEnabled", "UserName", "WebsiteLink", "YoutubeLink" },
                values: new object[,]
                {
                    { 1, "Admin User of ProgrammersBlog", 0, "d1f2f800-5ba8-4277-8413-a2022820813b", "adminuser@gmail.com", true, "https://facebook.com/adminuser", "Admin", "https://github.com/adminuser", "https://instagram.com/adminuser", "User", "https://linkedin.com/adminuser", false, null, "ADMINUSER@GMAIL.COM", "ADMINUSER", "AQAAAAIAAYagAAAAEHkQ7fBMK24wXC7cLPx0Zv+maLTd4Cbg6euMVPvYe7HVu36bl264IW3pHaGOuyhpqQ==", "+905555555555", true, "/UserImages/defaultUser.png", "c9247b3b-43c3-4a4d-89f3-db290ad2963f", "https://twitter.com/adminuser", false, "adminuser", "https://programmersblog.com/", "https://youtube.com/adminuser" },
                    { 2, "Editor User of ProgrammersBlog", 0, "aa1f0b9a-e382-4ba1-9cb9-08e6c7be83c2", "editoruser@gmail.com", true, "https://facebook.com/editoruser", "Admin", "https://github.com/editoruser", "https://instagram.com/editoruser", "User", "https://linkedin.com/editoruser", false, null, "EDITORUSER@GMAIL.COM", "EDITORUSER", "AQAAAAIAAYagAAAAEBjFAtEJK5twoSjRNmWbz84eiEL2ai4Yf4cG5yELLwvDdfnNsmLWYrEur6nnKlwbxA==", "+905555555555", true, "/UserImages/defaultUser.png", "ea05e8f2-a2dd-4df2-bc8b-564e59e010fc", "https://twitter.com/editoruser", false, "editoruser", "https://programmersblog.com/", "https://youtube.com/editoruser" }
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "ID", "ArticleComment", "ArticleLike", "ArticleThumbnail", "ArticleView", "CategoryId", "Content", "CreatedByName", "CreatedDate", "IsActive", "IsDeleted", "ModifiedByName", "ModifiedDate", "Note", "SeoAuthor", "SeoDescription", "SeoTags", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, 0, 1, "PostImages/defaultThumbnail.jpg", 100, 1, "Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı 1500'lerden beri endüstri standardı sahte metinler olarak kullanılmıştır. Beşyüz yıl boyunca varlığını sürdürmekle kalmamış, aynı zamanda pek değişmeden elektronik dizgiye de sıçramıştır. 1960'larda Lorem Ipsum pasajları da içeren Letraset yapraklarının yayınlanması ile ve yakın zamanda Aldus PageMaker gibi Lorem Ipsum sürümleri içeren masaüstü yayıncılık yazılımları ile popüler olmuştur.", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9307), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9314), "C# 9.0 ve .NET 5 Yenilikleri", "Alper Tunga", "C# 9.0 ve .NET 5 Yenilikleri", "C#, C# 9, .NET5, .NET Framework, .NET Core", "C# 9.0 ve .NET 5 Yenilikleri", 1 },
                    { 2, 0, 1, "PostImages/defaultThumbnail.jpg", 295, 2, "Yinelenen bir sayfa içeriğinin okuyucunun dikkatini dağıttığı bilinen bir gerçektir. Lorem Ipsum kullanmanın amacı, sürekli 'buraya metin gelecek, buraya metin gelecek' yazmaya kıyasla daha dengeli bir harf dağılımı sağlayarak okunurluğu artırmasıdır. Şu anda birçok masaüstü yayıncılık paketi ve web sayfa düzenleyicisi, varsayılan mıgır metinler olarak Lorem Ipsum kullanmaktadır. Ayrıca arama motorlarında 'lorem ipsum' anahtar sözcükleri ile arama yapıldığında henüz tasarım aşamasında olan çok sayıda site listelenir. Yıllar içinde, bazen kazara, bazen bilinçli olarak (örneğin mizah katılarak), çeşitli sürümleri geliştirilmiştir.", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9332), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9338), "C++ 11 ve 19 Yenilikleri", "Alper Tunga", "C++ 11 ve 19 Yenilikleri", "C++ 11 ve 19 Yenilikleri", "C++ 11 ve 19 Yenilikleri", 1 },
                    { 3, 0, 1, "PostImages/defaultThumbnail.jpg", 12, 3, "Yaygın inancın tersine, Lorem Ipsum rastgele sözcüklerden oluşmaz. Kökleri M.Ö. 45 tarihinden bu yana klasik Latin edebiyatına kadar uzanan 2000 yıllık bir geçmişi vardır. Virginia'daki Hampden-Sydney College'dan Latince profesörü Richard McClintock, bir Lorem Ipsum pasajında geçen ve anlaşılması en güç sözcüklerden biri olan 'consectetur' sözcüğünün klasik edebiyattaki örneklerini incelediğinde kesin bir kaynağa ulaşmıştır. Lorm Ipsum, Çiçero tarafından M.Ö. 45 tarihinde kaleme alınan \"de Finibus Bonorum et Malorum\" (İyi ve Kötünün Uç Sınırları) eserinin 1.10.32 ve 1.10.33 sayılı bölümlerinden gelmektedir. Bu kitap, ahlak kuramı üzerine bir tezdir ve Rönesans döneminde çok popüler olmuştur. Lorem Ipsum pasajının ilk satırı olan \"Lorem ipsum dolor sit amet\" 1.10.32 sayılı bölümdeki bir satırdan gelmektedir. 1500'lerden beri kullanılmakta olan standard Lorem Ipsum metinleri ilgilenenler için yeniden üretilmiştir. Çiçero tarafından yazılan 1.10.32 ve 1.10.33 bölümleri de 1914 H. Rackham çevirisinden alınan İngilizce sürümleri eşliğinde özgün biçiminden yeniden üretilmiştir.", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9354), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9357), "JavaScript ES2019 ve ES2020 Yenilikleri", "Alper Tunga", "JavaScript ES2019 ve ES2020 Yenilikleri", "JavaScript ES2019 ve ES2020 Yenilikleri", "JavaScript ES2019 ve ES2020 Yenilikleri", 1 },
                    { 4, 0, 1, "PostImages/defaultThumbnail.jpg", 666, 4, "É um facto estabelecido de que um leitor é distraído pelo conteúdo legível de uma página quando analisa a sua mancha gráfica. Logo, o uso de Lorem Ipsum leva a uma distribuição mais ou menos normal de letras, ao contrário do uso de 'Conteúdo aqui,conteúdo aqui'', tornando-o texto legível. Muitas ferramentas de publicação electrónica e editores de páginas web usam actualmente o Lorem Ipsum como o modelo de texto usado por omissão, e uma pesquisa por 'lorem ipsum' irá encontrar muitos websites ainda na sua infância. Várias versões têm evoluído ao longo dos anos, por vezes por acidente, por vezes propositadamente (como no caso do humor).", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9371), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9374), "Typescript 4.1 Yenilikleri", "Alper Tunga", "Typescript 4.1, Typescript, TYPESCRIPT 2021", "Typescript 4.1 Güncellemeleri", "Typescript 4.1", 1 },
                    { 5, 0, 1, "PostImages/defaultThumbnail.jpg", 3225, 5, "Yaygın inancın tersine, Lorem Ipsum rastgele sözcüklerden oluşmaz. Kökleri M.Ö. 45 tarihinden bu yana klasik Latin edebiyatına kadar uzanan 2000 yıllık bir geçmişi vardır. Virginia'daki Hampden-Sydney College'dan Latince profesörü Richard McClintock, bir Lorem Ipsum pasajında geçen ve anlaşılması en güç sözcüklerden biri olan 'consectetur' sözcüğünün klasik edebiyattaki örneklerini incelediğinde kesin bir kaynağa ulaşmıştır. Lorm Ipsum, Çiçero tarafından M.Ö. 45 tarihinde kaleme alınan \"de Finibus Bonorum et Malorum\" (İyi ve Kötünün Uç Sınırları) eserinin 1.10.32 ve 1.10.33 sayılı bölümlerinden gelmektedir. Bu kitap, ahlak kuramı üzerine bir tezdir ve Rönesans döneminde çok popüler olmuştur. Lorem Ipsum pasajının ilk satırı olan \"Lorem ipsum dolor sit amet\" 1.10.32 sayılı bölümdeki bir satırdan gelmektedir. 1500'lerden beri kullanılmakta olan standard Lorem Ipsum metinleri ilgilenenler için yeniden üretilmiştir. Çiçero tarafından yazılan 1.10.32 ve 1.10.33 bölümleri de 1914 H. Rackham çevirisinden alınan İngilizce sürümleri eşliğinde özgün biçiminden yeniden üretilmiştir.", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9388), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9391), "JAVA", "Alper Tunga", "Java, Android, Mobile, Kotlin, Uygulama Geliştirme", "Java, Mobil, Kotlin, Android, IOS, SWIFT", "Java ve Android'in Geleceği | 2021", 1 },
                    { 6, 0, 1, "PostImages/defaultThumbnail.jpg", 9999, 6, "Le Lorem Ipsum est simplement du faux texte employé dans la composition et la mise en page avant impression. Le Lorem Ipsum est le faux texte standard de l'imprimerie depuis les années 1500, quand un imprimeur anonyme assembla ensemble des morceaux de texte pour réaliser un livre spécimen de polices de texte. Il n'a pas fait que survivre cinq siècles, mais s'est aussi adapté à la bureautique informatique, sans que son contenu n'en soit modifié. Il a été popularisé dans les années 1960 grâce à la vente de feuilles Letraset contenant des passages du Lorem Ipsum, et, plus récemment, par son inclusion dans des applications de mise en page de texte, comme Aldus PageMaker.", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9406), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9409), "Python", "Alper Tunga", "Python ile Veri Madenciliği", "Python, Veri Madenciliği Nasıl Yapılır?", "Python ile Veri Madenciliği | 2021", 1 },
                    { 7, 0, 1, "PostImages/defaultThumbnail.jpg", 4818, 7, "Contrairement à une opinion répandue, le Lorem Ipsum n'est pas simplement du texte aléatoire. Il trouve ses racines dans une oeuvre de la littérature latine classique datant de 45 av. J.-C., le rendant vieux de 2000 ans. Un professeur du Hampden-Sydney College, en Virginie, s'est intéressé à un des mots latins les plus obscurs, consectetur, extrait d'un passage du Lorem Ipsum, et en étudiant tous les usages de ce mot dans la littérature classique, découvrit la source incontestable du Lorem Ipsum. Il provient en fait des sections 1.10.32 et 1.10.33 du 0De Finibus Bonorum et Malorum' (Des Suprêmes Biens et des Suprêmes Maux) de Cicéron. Cet ouvrage, très populaire pendant la Renaissance, est un traité sur la théorie de l'éthique. Les premières lignes du Lorem Ipsum, 'Lorem ipsum dolor sit amet...'', proviennent de la section 1.10.32", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9436), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9441), "PHP", "Alper Tunga", "Php ile API Oluşturma Rehberi", "php, laravel, api, oop", "Php Laravel Başlangıç Rehberi | API", 1 },
                    { 8, 0, 1, "PostImages/defaultThumbnail.jpg", 750, 8, "Plusieurs variations de Lorem Ipsum peuvent être trouvées ici ou là, mais la majeure partie d'entre elles a été altérée par l'addition d'humour ou de mots aléatoires qui ne ressemblent pas une seconde à du texte standard. Si vous voulez utiliser un passage du Lorem Ipsum, vous devez être sûr qu'il n'y a rien d'embarrassant caché dans le texte. Tous les générateurs de Lorem Ipsum sur Internet tendent à reproduire le même extrait sans fin, ce qui fait de lipsum.com le seul vrai générateur de Lorem Ipsum. Iil utilise un dictionnaire de plus de 200 mots latins, en combinaison de plusieurs structures de phrases, pour générer un Lorem Ipsum irréprochable. Le Lorem Ipsum ainsi obtenu ne contient aucune répétition, ni ne contient des mots farfelus, ou des touches d'humour.", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9506), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9509), "Kotlin", "Alper Tunga", "Kotlin ile Mobil Programlama Baştan Sona Adım Adım", "kotlin, android, mobil, programlama", "Kotlin ile Mobil Programlama", 1 },
                    { 9, 0, 1, "PostImages/defaultThumbnail.jpg", 14900, 9, "Al contrario di quanto si pensi, Lorem Ipsum non è semplicemente una sequenza casuale di caratteri. Risale ad un classico della letteratura latina del 45 AC, cosa che lo rende vecchio di 2000 anni. Richard McClintock, professore di latino al Hampden-Sydney College in Virginia, ha ricercato una delle più oscure parole latine, consectetur, da un passaggio del Lorem Ipsum e ha scoperto tra i vari testi in cui è citata, la fonte da cui è tratto il testo, le sezioni 1.10.32 and 1.10.33 del 'de Finibus Bonorum et Malorum' di Cicerone. Questo testo è un trattato su teorie di etica, molto popolare nel Rinascimento. La prima riga del Lorem Ipsum, 'Lorem ipsum dolor sit amet..'', è tratta da un passaggio della sezione 1.10.32.", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9523), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9527), "Swift", "Alper Tunga", "Swift ile IOS Mobil Programlama Baştan Sona Adım Adım", "IOS, android, mobil, programlama", "Swift ile IOS Programlama", 1 },
                    { 10, 0, 1, "PostImages/defaultThumbnail.jpg", 26777, 10, "Esistono innumerevoli variazioni dei passaggi del Lorem Ipsum, ma la maggior parte hanno subito delle variazioni del tempo, a causa dell’inserimento di passaggi ironici, o di sequenze casuali di caratteri palesemente poco verosimili. Se si decide di utilizzare un passaggio del Lorem Ipsum, è bene essere certi che non contenga nulla di imbarazzante. In genere, i generatori di testo segnaposto disponibili su internet tendono a ripetere paragrafi predefiniti, rendendo questo il primo vero generatore automatico su intenet. Infatti utilizza un dizionario di oltre 200 vocaboli latini, combinati con un insieme di modelli di strutture di periodi, per generare passaggi di testo verosimili. Il testo così generato è sempre privo di ripetizioni, parole imbarazzanti o fuori luogo ecc.", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9540), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 335, DateTimeKind.Local).AddTicks(9543), "Ruby", "Alper Tunga", "Ruby, Ruby on Rails Web Programlama, AirBnb Klon", "Ruby on Rails, Ruby, Web Programlama, AirBnb", "Ruby on Rails ile AirBnb Klon Kodlayalım", 1 }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 6, 1 },
                    { 7, 1 },
                    { 8, 1 },
                    { 9, 1 },
                    { 10, 1 },
                    { 11, 1 },
                    { 12, 1 },
                    { 13, 1 },
                    { 14, 1 },
                    { 15, 1 },
                    { 16, 1 },
                    { 17, 1 },
                    { 18, 1 },
                    { 19, 1 },
                    { 20, 1 },
                    { 21, 1 },
                    { 22, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 },
                    { 4, 2 },
                    { 5, 2 },
                    { 6, 2 },
                    { 7, 2 },
                    { 8, 2 },
                    { 17, 2 },
                    { 18, 2 },
                    { 19, 2 },
                    { 20, 2 },
                    { 21, 2 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "ID", "ArticleId", "CommentContent", "CreatedByName", "CreatedDate", "IsActive", "IsDeleted", "ModifiedByName", "ModifiedDate", "Note" },
                values: new object[,]
                {
                    { 1, 1, "Lorem Ipsum pasajlarının birçok çeşitlemesi vardır. Ancak bunların büyük bir çoğunluğu mizah katılarak veya rastgele sözcükler eklenerek değiştirilmişlerdir. Eğer bir Lorem Ipsum pasajı kullanacaksanız, metin aralarına utandırıcı sözcükler gizlenmediğinden emin olmanız gerekir. İnternet'teki tüm Lorem Ipsum üreteçleri önceden belirlenmiş metin bloklarını yineler. Bu da, bu üreteci İnternet üzerindeki gerçek Lorem Ipsum üreteci yapar. Bu üreteç, 200'den fazla Latince sözcük ve onlara ait cümle yapılarını içeren bir sözlük kullanır. Bu nedenle, üretilen Lorem Ipsum metinleri yinelemelerden, mizahtan ve karakteristik olmayan sözcüklerden uzaktır.", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5698), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5701), "C# Makale Yorumu" },
                    { 2, 2, "Lorem Ipsum jest tekstem stosowanym jako przykładowy wypełniacz w przemyśle poligraficznym. Został po raz pierwszy użyty w XV w. przez nieznanego drukarza do wypełnienia tekstem próbnej książki. Pięć wieków później zaczął być używany przemyśle elektronicznym, pozostając praktycznie niezmienionym. Spopularyzował się w latach 60. XX w. wraz z publikacją arkuszy Letrasetu, zawierających fragmenty Lorem Ipsum, a ostatnio z zawierającym różne wersje Lorem Ipsum oprogramowaniem przeznaczonym do realizacji druków na komputerach osobistych, jak Aldus PageMaker", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5714), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5717), "C++ Makale Yorumu" },
                    { 3, 3, "Ang Lorem Ipsum ay ginagamit na modelo ng industriya ng pagpriprint at pagtytypeset. Ang Lorem Ipsum ang naging regular na modelo simula pa noong 1500s, noong may isang di kilalang manlilimbag and kumuha ng galley ng type at ginulo ang pagkaka-ayos nito upang makagawa ng libro ng mga type specimen. Nalagpasan nito hindi lang limang siglo, kundi nalagpasan din nito ang paglaganap ng electronic typesetting at nanatiling parehas. Sumikat ito noong 1960s kasabay ng pag labas ng Letraset sheets na mayroong mga talata ng Lorem Ipsum, at kamakailan lang sa mga desktop publishing software tulad ng Aldus Pagemaker ginamit ang mga bersyon ng Lorem Ipsum.", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5729), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5732), "JavaScript Makale Yorumu" },
                    { 4, 4, "Lorem Ipsum er rett og slett dummytekst fra og for trykkeindustrien. Lorem Ipsum har vært bransjens standard for dummytekst helt siden 1500-tallet, da en ukjent boktrykker stokket en mengde bokstaver for å lage et prøveeksemplar av en bok. Lorem Ipsum har tålt tidens tann usedvanlig godt, og har i tillegg til å bestå gjennom fem århundrer også tålt spranget over til elektronisk typografi uten vesentlige endringer. Lorem Ipsum ble gjort allment kjent i 1960-årene ved lanseringen av Letraset-ark med avsnitt fra Lorem Ipsum, og senere med sideombrekkingsprogrammet Aldus PageMaker som tok i bruk nettopp Lorem Ipsum for dummytekst.", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5743), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5746), "Typescript Makale Yorumu" },
                    { 5, 5, "Lorem Ipsum este pur şi simplu o machetă pentru text a industriei tipografice. Lorem Ipsum a fost macheta standard a industriei încă din secolul al XVI-lea, când un tipograf anonim a luat o planşetă de litere şi le-a amestecat pentru a crea o carte demonstrativă pentru literele respective. Nu doar că a supravieţuit timp de cinci secole, dar şi a facut saltul în tipografia electronică practic neschimbată. A fost popularizată în anii '60 odată cu ieşirea colilor Letraset care conţineau pasaje Lorem Ipsum, iar mai recent, prin programele de publicare pentru calculator, ca Aldus PageMaker care includeau versiuni de Lorem Ipsum.", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5758), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5761), "Java Makale Yorumu" },
                    { 6, 6, "Lorem Ipsum je jednostavno probni tekst koji se koristi u tiskarskoj i slovoslagarskoj industriji. Lorem Ipsum postoji kao industrijski standard još od 16-og stoljeća, kada je nepoznati tiskar uzeo tiskarsku galiju slova i posložio ih da bi napravio knjigu s uzorkom tiska. Taj je tekst ne samo preživio pet stoljeća, već se i vinuo u svijet elektronskog slovoslagarstva, ostajući u suštini nepromijenjen. Postao je popularan tijekom 1960-ih s pojavom Letraset listova s odlomcima Lorem Ipsum-a, a u skorije vrijeme sa software-om za stolno izdavaštvo kao što je Aldus PageMaker koji također sadrži varijante Lorem Ipsum-a.", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5772), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5775), "Python Makale Yorumu" },
                    { 7, 7, "Lorem Ipsum – tas ir teksta salikums, kuru izmanto poligrāfijā un maketēšanas darbos. Lorem Ipsum ir kļuvis par vispārpieņemtu teksta aizvietotāju kopš 16. gadsimta sākuma. Tajā laikā kāds nezināms iespiedējs izveidoja teksta fragmentu, lai nodrukātu grāmatu ar burtu paraugiem. Tas ir ne tikai pārdzīvojis piecus gadsimtus, bet bez ievērojamām izmaiņām saglabājies arī mūsdienās, pārejot uz datorizētu teksta apstrādi. Tā popularizēšanai 60-tajos gados kalpoja Letraset burtu paraugu publicēšana ar Lorem Ipsum teksta fragmentiem un, nesenā pagātnē, tādas maketēšanas programmas kā Aldus PageMaker, kuras šablonu paraugos ir izmantots Lorem Ipsum teksts.", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5785), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5788), "Php Makale Yorumu" },
                    { 8, 8, "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5798), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5801), "Kotlin Makale Yorumu" },
                    { 9, 9, "هنالك العديد من الأنواع المتوفرة لنصوص لوريم إيبسوم، ولكن الغالبية تم تعديلها بشكل ما عبر إدخال بعض النوادر أو الكلمات العشوائية إلى النص. إن كنت تريد أن تستخدم نص لوريم إيبسوم ما، عليك أن تتحقق أولاً أن ليس هناك أي كلمات أو عبارات محرجة أو غير لائقة مخبأة في هذا النص. بينما تعمل جميع مولّدات نصوص لوريم إيبسوم على الإنترنت على إعادة تكرار مقاطع من نص لوريم إيبسوم نفسه عدة مرات بما تتطلبه الحاجة، يقوم مولّدنا هذا باستخدام كلمات من قاموس يحوي على أكثر من 200 كلمة لا تينية، مضاف إليها مجموعة من الجمل النموذجية، لتكوين نص لوريم إيبسوم ذو شكل منطقي قريب إلى النص الحقيقي. وبالتالي يكون النص الناتح خالي من التكرار، أو أي كلمات أو عبارات غير لائقة أو ما شابه. وهذا ما يجعله أول مولّد نص لوريم إيبسوم حقيقي على الإنترنت.", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5812), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5815), "Swift Makale Yorumu" },
                    { 10, 10, "Lorem Ipsum，也称乱数假文或者哑元文本， 是印刷及排版领域所常用的虚拟文字。由于曾经一台匿名的打印机刻意打乱了一盒印刷字体从而造出一本字体样品书，Lorem Ipsum从西元15世纪起就被作为此领域的标准文本使用。它不仅延续了五个世纪，还通过了电子排版的挑战，其雏形却依然保存至今。在1960年代，”Leatraset”公司发布了印刷着Lorem Ipsum段落的纸张，从而广泛普及了它的使用。最近，计算机桌面出版软件”Aldus PageMaker”也通过同样的方式使Lorem Ipsum落入大众的视野。", "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5825), true, false, "InitialCreate", new DateTime(2022, 12, 30, 0, 50, 38, 337, DateTimeKind.Local).AddTicks(5828), "Ruby Makale Yorumu" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_UserId",
                table: "Articles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
