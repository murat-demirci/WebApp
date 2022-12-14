using Entities.Dtos;

namespace Mvc.Areas.Admin.Models
{
    public class UserUpdateAjaxViewModel
    {
        public string UserUpdatePartial { get; set; }
        public UserUpdateDto UserUpdateDto { get; set; }
        public UserDto UserDto { get; set; }
    }
}
