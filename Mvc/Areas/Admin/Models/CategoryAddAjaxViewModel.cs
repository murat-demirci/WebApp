using Entities.Dtos;
using Entities.Dtos.Categories;

namespace Mvc.Areas.Admin.Models
{
    public class CategoryAddAjaxViewModel
    {
        //mvc katmanini ilgilendiren ve sadece mvc katmaninda kullanilacak class
        //sadece ajax islemleri icin olusturuldu
        public CategoryAddDto CategoryAddDto { get; set; }
        public string CategoryAddPartial { get; set; }
        //ajax post islemi valid dogru degilse olusan hatalari iletmek icin kullanilir
        public CategoryDto CategoryDto { get; set; }
    }
}
