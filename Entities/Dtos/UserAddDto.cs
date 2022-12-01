using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos.User
{
    public class UserAddDto
    {
        [DisplayName("Kullanici Adi")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]//{0} display name i gosteriyor
        [MaxLength(50, ErrorMessage = "{0}, maksimum {1} karakter alabilir")]
        [MinLength(3, ErrorMessage = "{0}, minimum {1} karakter alabilir")]
        public string UserNmae { get; set; }
        [DisplayName("E-Posta Adresi")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]//{0} display name i gosteriyor
        [MaxLength(100, ErrorMessage = "{0}, maksimum {1} karakter alabilir")]
        [MinLength(10, ErrorMessage = "{0}, minimum {1} karakter alabilir")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Parola")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]//{0} display name i gosteriyor
        [MaxLength(30, ErrorMessage = "{0}, maksimum {1} karakter alabilir")]
        [MinLength(6, ErrorMessage = "{0}, minimum {1} karakter alabilir")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Kullanici Resmi")]
        [Required(ErrorMessage = "Lutfen bir {0} seciniz")]//{0} display name i gosteriyor
        [DataType(DataType.Upload)]
        public IFormFile UserPicture { get; set; }
    }
}
