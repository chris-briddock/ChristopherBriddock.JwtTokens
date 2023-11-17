using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ChristopherBriddock.JwtTokens.Exceptions;
using ChristopherBriddock.JwtTokens.Models;

namespace ChristopherBriddock.JwtTokens
{
    /// <summary>
    /// Represents a service for managing JSON Web Tokens.
    /// This includes creating, refreshing, validating, and revoking JSON Web Tokens.
    /// </summary>
    public class JsonWebTokens : IJsonWebTokens
    {
        /// <summary>
        /// Tries to create a JWT (JSON Web Token) asynchronously.
        /// </summary>
        /// <param name="email">The email of the token's recipient.</param>
        /// <param name="jwtSecret">The secret key used to sign the JWT.</param>
        /// <param name="issuer">The issuer of the JWT.</param>
        /// <param name="audience">The intended audience of the JWT.</param>
        /// <param name="expires">The expiration date and time of the JWT.</param>
        /// <param name="subject">The subject of the JWT.</param>
        /// <returns>A <see cref="JwtResult"/> containing the result of the token creation.</returns>
        public async Task<JwtResult> TryCreateTokenAsync(string email,
                                                         string jwtSecret,
                                                         string issuer,
                                                         string audience,
                                                         string expires,
                                                         string subject)
        {
            JwtResult result = new();
            try
            {

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtSecret);

                IList<Claim> claims = new List<Claim>()
                {
                    new(JwtRegisteredClaimNames.Sub, subject),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                    new(JwtRegisteredClaimNames.Email, email)

                };
                var expiryMinutesToAdd = Convert.ToInt16(expires);
                JwtSecurityToken tokenDescriptor = new(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expiryMinutesToAdd),
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
        /// Tries to validate a JWT (JSON Web Token) asynchronously.
        /// </summary>
        /// <param name="token">The JWT to validate.</param>
        /// <param name="jwtSecret">The secret key used to validate the JWT's signature.</param>
        /// <param name="issuer">The expected issuer of the JWT.</param>
        /// <param name="audience">The expected audience of the JWT.</param>
        /// <returns>A <see cref="JwtResult"/> containing the result of the token validation.</returns>
        public async Task<JwtResult> TryValidateTokenAsync(string token,
                                                           string jwtSecret,
                                                           string issuer,
                                                           string audience)
        {
            JwtResult result = new();
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

                TokenValidationResult validationResult = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);


                // If the validation succeeds, the JWT is both well-formed and has a valid signature.
                if (validationResult.IsValid)
                {
                    result.Success = true;
                    result.Token = token;
                }

                return result;
            }
            catch (SecurityTokenException ex)
            {
                result.Error = ex.Message;
                result.Success = false;
                throw;
            }
        }
    }
}