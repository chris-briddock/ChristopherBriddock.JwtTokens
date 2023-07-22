using System;

namespace ChristopherBriddock.JwtTokens
{
    public class CreateJwtException : Exception
    {
        public CreateJwtException()
        {
        }

        public CreateJwtException(string message)
            : base(message)
        {
        }

        public CreateJwtException(string message, Exception inner)
            : base(message, inner)
        {
        }
        
    }
}