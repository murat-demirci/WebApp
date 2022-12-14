using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public class UserLoginDto
    {
        [DisplayName("E-Posta Adresi")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]//{0} display name i gosteriyor
        [MaxLength(100, ErrorMessage = "{0}, maksimum {1} karakter alabilir")]
        [MinLength(10, ErrorMessage = "{0}, minimum {1} karakter alabilir")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Parola")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]//{0} display name i gosteriyor
        [MaxLength(100, ErrorMessage = "{0}, maksimum {1} karakter alabilir")]
        [MinLength(6, ErrorMessage = "{0}, minimum {1} karakter alabilir")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Beni hatirla")]
        public bool RememberMe { get; set; }
    }
}
