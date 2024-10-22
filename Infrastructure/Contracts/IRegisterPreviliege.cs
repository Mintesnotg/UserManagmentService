using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface IRegisterPreviliege
    {
        void RegisterPrivileges(Assembly assembly);
    }
}
