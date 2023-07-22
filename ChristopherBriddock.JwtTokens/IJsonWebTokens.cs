using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChristopherBriddock.JwtTokens
{
    public interface IJsonWebTokens
    {
        
        Task<JwtResult> TryCreateTokenAsync(string email,
                                            string jwtSecret,
                                            string issuer,
                                            string audience,
                                            int expires,
                                            string subject);
        Task<JwtResult> TryValidateTokenAsync(string token,
                                              string jwtSecret,
                                              string issuer,
                                              string audience);

    }
}