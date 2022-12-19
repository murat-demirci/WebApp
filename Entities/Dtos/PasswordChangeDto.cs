using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public class PasswordChangeDto
    {
        [DisplayName("Eski Parola")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]//{0} display name i gosteriyor
        [MaxLength(100, ErrorMessage = "{0}, maksimum {1} karakter alabilir")]
        [MinLength(6, ErrorMessage = "{0}, minimum {1} karakter alabilir")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [DisplayName("Yeni Parola")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]//{0} display name i gosteriyor
        [MaxLength(100, ErrorMessage = "{0}, maksimum {1} karakter alabilir")]
        [MinLength(6, ErrorMessage = "{0}, minimum {1} karakter alabilir")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DisplayName("Yeni Parola (Kontrol)")]
        [Required(ErrorMessage = "Lutfen bir {0} giriniz")]//{0} display name i gosteriyor
        [MaxLength(100, ErrorMessage = "{0}, maksimum {1} karakter alabilir")]
        [MinLength(6, ErrorMessage = "{0}, minimum {1} karakter alabilir")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Parola uyusmadi")]
        public string CheckPassword { get; set; }
    }
}
