using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dtos
{
    public class UserRoleMappingDto
    {
        public string UserId { get; set; } = string.Empty;
        public List<string> UserRoleId { get; set; } = [];
    }
}
