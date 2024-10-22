using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dtos
{
    public class ControllerWithAction
    {
        public Type ControllerType { get; set; }
        public string ControllerName { get; set; } = string.Empty;
        public List<string> Actions { get; set; }
    }
}
