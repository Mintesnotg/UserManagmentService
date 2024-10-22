using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class RolePrivilege
    {
        public string RoleId { get; set; } = string.Empty;
        public UserRole Role { get; set; } = new UserRole();
        public string PrivilegeId { get; set; } = string.Empty;
        public Privilege Privilege { get; set; } = new Privilege();
     

      
    }
}
