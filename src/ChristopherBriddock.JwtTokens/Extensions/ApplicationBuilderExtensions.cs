using Microsoft.AspNetCore.Builder;
using System;

namespace ChristopherBriddock.JwtTokens.Extensions
{
    /// <summary>
    /// A collection of extension methods that extend the functionality of <see cref="IApplicationBuilder"/>
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Middleware for generating a Json Web Token (JWT) when the token endpoint is called.
        /// </summary>
        /// <remarks>
        /// This middleware is used to generate and issue a JWT when a specific endpoint is accessed.
        /// It intercepts requests to the "/token" endpoint and adds a JWT to the response if conditions are met.
        /// </remarks>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <returns>The modified <see cref="IApplicationBuilder"/> instance with JWT generation middleware added.</returns>
        public static IApplicationBuilder UseJwtGenerator(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(nameof(app));

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/token"), appBuilder =>
            appBuilder.UseMiddleware<JwtMiddleware>());

            return app;
        }

    }
}