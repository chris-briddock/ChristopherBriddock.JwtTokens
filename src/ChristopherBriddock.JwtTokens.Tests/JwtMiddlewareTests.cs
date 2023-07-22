using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ChristopherBriddock.JwtTokens.Tests
{
    public class JwtMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_ShouldAddTokenToResponseHeader_WhenTokenIsValid()
        {
            // Arrange
            var mockRequestDelegate = new Mock<RequestDelegate>();
            var mockJsonWebTokens = new Mock<IJsonWebTokens>();
            var mockConfiguration = new Mock<IConfiguration>();

            // Set up the configuration
            var configValues = new Dictionary<string, string>
        {
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" },
            { "Jwt:Secret", "TestSecret" },
            { "Jwt:Subject", "TestSubject" },
            { "Jwt:Expires", "60" }
        };
            mockConfiguration.Setup(c => c[It.IsAny<string>()]).Returns<string>(k => configValues[k]);

            // Set up the JsonWebTokens
            var jwtResult = new JwtResult { Success = true, Token = "testToken" };
            mockJsonWebTokens.Setup(j => j.TryCreateTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(jwtResult);
            mockJsonWebTokens.Setup(j => j.TryValidateTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(jwtResult);

            // Set up the HttpContext
            var context = new DefaultHttpContext();
            var claimsIdentity = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Email, "test@test.com") });
            context.User = new ClaimsPrincipal(claimsIdentity);

            var jwtMiddleware = new JwtMiddleware(mockRequestDelegate.Object, mockJsonWebTokens.Object, mockConfiguration.Object);

            // Act
            await jwtMiddleware.InvokeAsync(context);

            // Assert
            Assert.Equal("Bearer testToken", context.Response.Headers["Authorization"].ToString());
        }
    }
}

