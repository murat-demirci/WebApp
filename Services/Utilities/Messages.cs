using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Services.Utilities
{
    //yapilan islemler sonucu geriye donulen islem mesajlarinin yonetilebilirligini
    //kolaylastirmak acisindan kullanilir
    public static class Messages
    {
        // Messages.Category.NotFound() kategoriler icin bulunamadi mesaji
        public static class Category
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural)
                {
                    return "Hicbir Kategori bulunamadi";
                }
                return "Kategori bulunamadi";
                //tek kategori icin
            }

            public static string Add(string categoryName)
            {
                return $"Eklenen kategori \n {categoryName}";
            }

            public static string Delete(string categoryName)
            {
                return $"Silinen kategori: \n{categoryName}";
            }

            public static string Remove(string categoryName)
            {
                return $"Kaldirilan kategori \n {categoryName}";
            }

            public static string Update(string categoryName)
            {
                return $"Guncellenen kategori \n {categoryName}";
            }
        }

        public static class Article
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural)
                {
                    return "Hicbir Makale bulunamadi";
                }
                return "Makale bulunamadi";
                //tek kategori icin
            }

            public static string Add(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla eklenmiştir.";
            }

            public static string Update(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla güncellenmiştir.";
            }

            public static string Remove(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla silinmiştir.";
            }

            public static string Delete(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla veritabanından silinmiştir.";
            }
        }

        public static class Comment
        {
            public static string Approve(int commentId)
            {
                return $"{commentId} no'lu yorum başarıyla onaylanmıştır";
            }
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiç bir yorum bulunamadı.";
                return "Böyle bir yorum bulunamadı.";
            }

            public static string Add(string createdByName)
            {
                return $"Sayın {createdByName}, yorumunuz başarıyla eklenmiştir.";
            }

            public static string Update(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla güncellenmiştir.";
            }
            public static string Delete(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla silinmiştir.";
            }
            public static string HardDelete(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla veritabanından silinmiştir.";
            }
        }
    }
}
