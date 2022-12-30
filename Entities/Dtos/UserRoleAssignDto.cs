using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UserRoleAssignDto
    {
        public UserRoleAssignDto()
        {
            RoleAssignDtos = new List<RoleAssignDto>();
        }

        //rol atanacak kullanıcının id'si
        public int UserId { get; set; }
        public string? UserName { get; set; }
        //atanacak roller, controller içinde atanacak, bundan dolayı constructor içinde initialize edilmeli
        public IList<RoleAssignDto> RoleAssignDtos { get; set; }
    }
}
