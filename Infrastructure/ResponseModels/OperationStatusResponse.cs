using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.Enums.Enumerators;

namespace Infrastructure.ResponseModels
{
    public class OperationStatusResponse
    {
        public string Message { get; set; } = String.Empty;
        public OperationStatus Status { get; set; }

    }
}
