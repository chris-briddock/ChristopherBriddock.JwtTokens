using System.Collections.Generic;
using System.Security.Claims;

namespace ChristopherBriddock.JwtTokens
{
    public interface IJsonWebTokens
    {
        string GenerateJwtToken(IEnumerable<Claim> claims,
                                string jwtSecret,
                                string issuer,
                                string audience,
                                string subject);
        bool IsValid(string token);
        //string RefreshToken(string token,
        //                    string jwtSecret,
        //                    string issuer,
        //                    string audience);
    }
}