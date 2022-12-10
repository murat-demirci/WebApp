using Entities.Dtos;

namespace Mvc.Areas.Admin.Models
{
    public class CategoryUpdateAjaxViewModel
    {
        //mvc katmanini ilgilendiren ve sadece mvc katmaninda kullanilacak class
        //sadece ajax islemleri icin olusturuldu
        public CategoryUpdateDto CategoryUpdateDto { get; set; }
        public string CategoryUpdatePartial { get; set; }
        //ajax post islemi valid dogru degilse olusan hatalari iletmek icin kullanilir
        public CategoryDto CategoryDto { get; set; }
    }
}
