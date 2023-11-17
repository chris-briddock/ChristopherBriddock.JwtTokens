using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Claims;
using ChristopherBriddock.JwtTokens.Exceptions;
using ChristopherBriddock.JwtTokens.Models;
using Microsoft.AspNetCore.Http;

namespace ChristopherBriddock.JwtTokens
{
    /// <summary>
    /// Middleware for working with JSON Web Tokens (JWT) in the context of HTTP requests.
    /// </summary>
    public class JwtMiddleware
    {
        /// <summary>
        /// The next HTTP request delegate for passing the request to the next middleware in the pipeline.
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// An instance of the <see cref="IJsonWebTokens"/> interface for performing JWT (JSON Web Token) operations.
        /// </summary>
        private readonly IJsonWebTokens _jsonWebTokens;
        /// <summary>
        /// The configuration containing JWT (JSON Web Token) settings, such as issuer, audience, secret, subject, and expiration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next HTTP request delegate.</param>
        /// <param name="jsonWebTokens">An instance of the <see cref="IJsonWebTokens"/> interface for JWT operations.</param>
        /// <param name="configuration">The configuration containing JWT settings.</param>
        public JwtMiddleware(RequestDelegate next,
                             IJsonWebTokens jsonWebTokens,
                             IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _jsonWebTokens = jsonWebTokens ?? throw new ArgumentNullException(nameof(jsonWebTokens));
        }

        /// <summary>
        /// Invokes the JWT middleware to handle JWT creation, validation, and setting the token in the response headers.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            ArgumentNullException.ThrowIfNull(nameof(context));

            JwtResult result;
            try
            {
                string email = context.User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
                string issuer = _configuration["Jwt:Issuer"]!;
                string audience = _configuration["Jwt:Audience"]!;
                string secret = _configuration["Jwt:Secret"]!;
                string subject = _configuration["Jwt:Subject"]!;
                string expires = _configuration["Jwt:Expires"]!;
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
                        context.Response.Headers.Append("Authorization", $"Bearer {result.Token}");
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