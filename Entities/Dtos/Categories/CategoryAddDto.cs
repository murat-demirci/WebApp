using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    //veri transfer objesidir
    //kullanici bize belirli degerleri verecek'
    //kalan degerler arka planda islenecek
    //tum alanlar acik olmayacak
    public class CategoryAddDto
    {
        //MVC katmaninda goruntuleme ayarlari
        [DisplayName("Kategori Adi")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]//{0} display name i gosteriyor
        [MaxLength(50, ErrorMessage = "{0}, maksimum {1} karakter alabilir")]
        [MinLength(1, ErrorMessage = "{0}, minimum {1} karakter alabilir")]
        public string Name { get; set; }
        [DisplayName("Not Alani")]
        [MaxLength(500, ErrorMessage = "{0}, maksimum {1} karakter alabilir")]
        [MinLength(5, ErrorMessage = "{0}, minimum {1} karakter alabilir")]
        public string Note { get; set; }
        [DisplayName("Aktif")]
        [Required(ErrorMessage = "Lutfen {0} durumu seciniz")]
        public bool isActive { get; set; }
    }
}
