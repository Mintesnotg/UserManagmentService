using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Privilege: AuditLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Action { get; set; }= string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<RolePrivilege> RolePrivileges { get; set; } = [];
    }

}
