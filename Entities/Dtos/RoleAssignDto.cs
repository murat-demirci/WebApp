using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class RoleAssignDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        //ilgili rolün kullanıcıda olup olmadığını kontrol eder
        public bool HasRole { get; set; }
    }
}
