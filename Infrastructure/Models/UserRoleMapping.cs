using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class UserRoleMapping: IdentityUserRole<string>
    {
        public DateTime AssignDate { get; set; }
        public string AssignedBy { get; set; }
    }
}
