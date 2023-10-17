using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace ChristopherBriddock.JwtTokens.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method for adding authentication services to the IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to which authentication services will be added.</param>
        /// <returns>The modified IServiceCollection instance.</returns>
        public static IServiceCollection AddCustomJwtAuth(this IServiceCollection services)
        {

            ArgumentNullException.ThrowIfNull(services, nameof(services));

            services.TryAddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();

            return services;
        }
    }
}