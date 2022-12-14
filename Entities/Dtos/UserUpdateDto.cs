using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public class UserUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [DisplayName("Kullanici Adi")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]//{0} display name i gosteriyor
        [MaxLength(50, ErrorMessage = "{0}, maksimum {1} karakter alabilir")]
        [MinLength(3, ErrorMessage = "{0}, minimum {1} karakter alabilir")]
        public string UserName { get; set; }
        [DisplayName("E-Posta Adresi")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]//{0} display name i gosteriyor
        [MaxLength(100, ErrorMessage = "{0}, maksimum {1} karakter alabilir")]
        [MinLength(10, ErrorMessage = "{0}, minimum {1} karakter alabilir")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Kullanici Resmini Guncelle")]
        [DataType(DataType.Upload)]//ifromupload oldugu icin datatype.upload olmasi onemli
        public IFormFile? UserPictureFile { get; set; }
        [DisplayName("Kullanici Resmi")]
        public string UserPicture { get; set; }//sadece dosya adini saklamak icin kullanilir
    }
}
