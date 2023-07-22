using System;

namespace ChristopherBriddock.JwtTokens
{
    public class ValidateJwtException : Exception
    {
        public ValidateJwtException()
        {
        }

        public ValidateJwtException(string message)
            : base(message)
        {
        }

        public ValidateJwtException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}