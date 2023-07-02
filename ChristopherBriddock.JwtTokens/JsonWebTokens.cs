using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChristopherBriddock.JwtTokens
{
    public sealed class JsonWebTokens : IJsonWebTokens
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public JsonWebTokens(JwtSecurityTokenHandler tokenHandler,
                             TokenValidationParameters tokenValidationParameters)
        {
            _tokenHandler = tokenHandler;
            _tokenValidationParameters = tokenValidationParameters;
        }
        public string GenerateJwtToken(IEnumerable<Claim> claims,
                                       string jwtSecret,
                                       string issuer,
                                       string audience,
                                       string subject)
        {
            try
            {
                // Set the token's expiration time
                DateTime expires = DateTime.UtcNow.AddDays(1);

                // Set the token's signing key and algorithm
                SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
                SigningCredentials signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                // Add the user's claims to the token
                List<Claim> jwtClaims = new List<Claim>(claims)
                {
                    new Claim(JwtRegisteredClaimNames.Sub, subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                };

                // Create the token
                JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: jwtClaims,
                    notBefore: DateTime.UtcNow,
                    expires: expires,
                    signingCredentials: signingCredentials
                );

                // Return the token as a string
                return _tokenHandler.WriteToken(jwt);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool IsValid(string token)
        {
            try
            {
                _ = _tokenHandler.ValidateToken(token, _tokenValidationParameters, out _);
                return true;
            }
            catch (SecurityTokenException)
            {
                return false;
            }

        }
    }
}
