using ChristopherBriddock.JwtTokens.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace ChristopherBriddock.JwtTokens
{
    /// <summary>
    /// Configures JWT Bearer authentication options using the Options pattern.
    /// </summary>
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureJwtBearerOptions"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public ConfigureJwtBearerOptions(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configures JWT Bearer options.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="options">The JWT Bearer authentication options to configure.</param>
        public void Configure(string? name, JwtBearerOptions options) => Configure(options);

        /// <summary>
        /// Configures JWT Bearer options.
        /// </summary>
        /// <param name="options">The JWT Bearer authentication options to configure.</param>
        public void Configure(JwtBearerOptions options)
        {
            // Retrieve the JWT secret from the application configuration
            string issuer = Configuration["Jwt:Issuer"]!;
            string audience = Configuration["Jwt:Audience"]!;
            string jwtSecret = Configuration["Jwt:Secret"]!;
            
            if (string.IsNullOrWhiteSpace(jwtSecret))
            {
                throw new JwtSecretNullOrEmptyException();
            }

            var key = Encoding.ASCII.GetBytes(jwtSecret);
            // Configure token validation parameters
            options.TokenValidationParameters = new TokenValidationParameters()
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

            // Save the token in the authentication context
            options.SaveToken = true;
        }
    }
}