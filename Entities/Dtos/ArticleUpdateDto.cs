using Entities.Concrete;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public class ArticleUpdateDto
    {
        [Required()]
        public int Id { get; set; }
        [DisplayName("Baslik")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]
        [MaxLength(100, ErrorMessage = "{0} alani maksimum {1} karakter alabilir")]
        [MinLength(1, ErrorMessage = "{0} alani minimum {1} karakter alabilir")]
        public string Title { get; set; }
        [DisplayName("Icerik")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]
        [MinLength(20, ErrorMessage = "{0} alani minimum {1} karakter alabilir")]
        public string Content { get; set; }
        [DisplayName("Icerik Resmi")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]
        [MaxLength(250, ErrorMessage = "{0} alani maksimum {1} karakter alabilir")]
        [MinLength(5, ErrorMessage = "{0} alani minimum {1} karakter alabilir")]
        public string ArticleThumbnail { get; set; }
        [DisplayName("Seo Yazar")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]
        [MaxLength(50, ErrorMessage = "{0} alani maksimum {1} karakter alabilir")]
        [MinLength(5, ErrorMessage = "{0} alani minimum {1} karakter alabilir")]
        public string SeoAuthor { get; set; }
        [DisplayName("Seo Aciklamasi")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]
        [MaxLength(150, ErrorMessage = "{0} alani maksimum {1} karakter alabilir")]
        [MinLength(1, ErrorMessage = "{0} alani minimum {1} karakter alabilir")]
        public string SeoDescription { get; set; }
        [DisplayName("Seo Etiketi")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]
        [MaxLength(70, ErrorMessage = "{0} alani maksimum {1} karakter alabilir")]
        [MinLength(5, ErrorMessage = "{0} alani minimum {1} karakter alabilir")]
        public string SeoTags { get; set; }
        [DisplayName("Kategori")]
        [Required(ErrorMessage = "Lutfen bir {0} seciniz")]
        public int categoryId { get; set; }
        public Category Category { get; set; }
        [DisplayName("Sil")]
        [Required(ErrorMessage = "Lutfen bir {0} durumu seciniz")]
        public bool isDeleted { get; set; }
        [DisplayName("Aktif")]
        [Required(ErrorMessage = "Lutfen bir {0} durumu seciniz")]
        public bool isActive { get; set; }
    }
}
