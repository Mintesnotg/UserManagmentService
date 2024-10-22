using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dtos
{
    public class AssignPrivilegesToRoleDto
    {

        public string RoleId { get; set; } = string.Empty;
        public List<string> PrivilegeIds { get; set; } = [];
    }
}
