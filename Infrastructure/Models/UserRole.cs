using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class UserRole : IdentityRole
    {
        public string RoleName { get; set; } = string.Empty;
        public DateTime RegisteredDate { get; set; }
        public string Description { get; set; }= string.Empty;
        public ICollection<RolePrivilege> RolePrivileges { get; set; } = [];

    }
}
