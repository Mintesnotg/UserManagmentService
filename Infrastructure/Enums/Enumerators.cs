using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Enums
{
    public class Enumerators
    {
        public enum RecordStatus
        {
            [Description("InActive")]
            Inactive = 1,
            [Description("Active")]
            Active = 2,
            [Description("Deleted")]
            Deleted = 3
        }

        public enum OperationStatus
        {
            ERROR,
            SUCCESS,
            EXIST,
            WARNING,
            UNAUTORIZED,
            EMPTY,
        }
    }
}
