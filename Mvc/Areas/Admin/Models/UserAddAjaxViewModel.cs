using Entities.Dtos;

namespace Mvc.Areas.Admin.Models
{
    public class UserAddAjaxViewModel
    {
        //mvc katmanini ilgilendiren ve sadece mvc katmaninda kullanilacak class
        //sadece ajax islemleri icin olusturuldu
        public UserAddDto UserAddDto { get; set; }
        public string UserAddPartial { get; set; }
        //ajax post islemi valid dogru degilse olusan hatalari iletmek icin kullanilir
        public UserDto UserDto { get; set; }
    }
}
