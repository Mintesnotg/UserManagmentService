﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dtos
{
    public class TokenApiModel
    {
        public string? AccessToken { get; set; } =string.Empty;
        public string? RefreshToken { get; set; } = string.Empty;
    }
}
