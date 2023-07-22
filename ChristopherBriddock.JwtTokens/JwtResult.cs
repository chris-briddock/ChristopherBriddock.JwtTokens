using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristopherBriddock.JwtTokens
{
    public class JwtResult
    {
        public bool Success { get; set; } = false;
        public string Token { get; set; } = string.Empty;
        public string? Error { get; set; } = null;
    }
}