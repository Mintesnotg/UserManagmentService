using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dtos
{
    public class UserRoleDto
    {
        public string RoleName { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

    }
}
