using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dtos
{
    public class RequestPasswordResetDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } =string.Empty;
    }
}
