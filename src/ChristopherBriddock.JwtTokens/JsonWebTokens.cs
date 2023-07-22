using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ChristopherBriddock.JwtTokens
{
    /// <summary>
    /// Represents a service for managing JSON Web Tokens.
    /// This includes creating, refreshing, validating, and revoking JSON Web Tokens.
    /// </summary>
    public class JsonWebTokens : IJsonWebTokens
    {
        /// <summary>
        /// Creates a JSON Web Token (JWT) asynchronously.
        /// </summary>
        /// <param name="claims">The claims to include in the JWT.</param>
        /// <param name="jwtSecret">The secret key for signing the JWT.</param>
        /// <param name="issuer">The issuer of the JWT.</param>
        /// <param name="audience">The audience of the JWT.</param>
        /// <param name="subject">The subject of the JWT.</param>
        /// <returns>A JwtResult that indicates the result of the JWT creation operation. If successful, the JwtResult includes the JWT as a string. If an error occurs, the JwtResult includes an error message.</returns>
        public async Task<JwtResult> TryCreateTokenAsync(string email,
                                                   string jwtSecret,
                                                   string issuer,
                                                   string audience,
                                                   int expires,
                                                   string subject)
        {
            JwtResult result = new JwtResult();
            try
            {

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtSecret);

                IList<Claim> claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToString(), ClaimValueTypes.Integer64),
                    new Claim(JwtRegisteredClaimNames.Email, email)

                };
                var tokenDescriptor = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expires),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature
                    )
                );
                result.Success = true;
                result.Token = tokenHandler.WriteToken(tokenDescriptor);
            }
            catch (CreateJwtException ex)
            {
                result.Error = ex.Message;
                result.Success = false;
                throw;
            }
            return await Task.FromResult(result);
        }

        /// <summary>
        /// Validates a JSON Web Token (JWT) asynchronously.
        /// </summary>
        /// <param name="token">The JWT to validate.</param>
        /// <param name="jwtSecret">The JWT Secret used for signing the JWT.</param>
        /// <param name="issuer">The Issuer of the JWT.</param>
        /// <param name="audience">The Audience of the JWT.</param>
        /// <returns>A JwtResult that indicates the result of the JWT validation operation. If successful, the JwtResult includes the validated JWT as a string. If an error occurs, the JwtResult includes an error message.</returns>
        public async Task<JwtResult> TryValidateTokenAsync(string token,
                                                           string jwtSecret,
                                                           string issuer,
                                                           string audience)
        {
            JwtResult result = new JwtResult();
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtSecret);
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                TokenValidationResult validationResult = await tokenHandler.ValidateTokenAsync(token,
                                                                                               tokenValidationParameters);
                if (validationResult.IsValid)
                {
                    result.Success = true;
                    result.Token = token;
                }
                else
                {
                    result.Success = false;
                }

                return await Task.FromResult(result);
            }
            catch (ValidateJwtException ex)
            {
                result.Error = ex.Message;
                result.Success = false;
                throw ex;
            }
        }

    }
}