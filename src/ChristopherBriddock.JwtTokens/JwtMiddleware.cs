using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Claims;

namespace ChristopherBriddock.JwtTokens
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJsonWebTokens _jsonWebTokens;
        private readonly IConfiguration _configuration;
        public JwtMiddleware(RequestDelegate next,
                             IJsonWebTokens jsonWebTokens,
                             IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _jsonWebTokens = jsonWebTokens ?? throw new ArgumentNullException(nameof(jsonWebTokens));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            JwtResult result;
            try 
            {
                string email = context.User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
                string issuer = _configuration["Jwt:Issuer"]!;
                string audience = _configuration["Jwt:Audience"]!;
                string secret = _configuration["Jwt:Secret"]!;
                string subject = _configuration["Jwt:Subject"]!;
                int expires = Convert.ToInt16(_configuration["Jwt:Expires"]!);
                result = await _jsonWebTokens.TryCreateTokenAsync(email,
                                                                  secret,
                                                                  issuer,
                                                                  audience,
                                                                  expires,
                                                                  subject);
                if (result.Success) 
                {
                    bool isValid = (await _jsonWebTokens.TryValidateTokenAsync(result.Token,
                                                                               secret,
                                                                               issuer,
                                                                               audience)).Success;
                    if (!isValid) 
                    {
                        throw new ValidateJwtException("Invalid token.");
                    }
                       
                    if (isValid) 
                    {
                        context.Response.Headers.Add("Authorization", $"Bearer {result.Token}");
                    }
                    
                }
                await _next(context);
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
