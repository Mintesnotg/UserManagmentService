using Infrastructure.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.Enums.Enumerators;

namespace Infrastructure.Dtos
{
    public class AuthenticatedResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public string ? Message { get; set; }
        public OperationStatus OperationStatus { get; set; }
    }
}
