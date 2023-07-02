using ChristopherBriddock.JwtTokens.Tests.Mocks;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChristopherBriddock.JwtTokens.Tests;

public sealed class JsonWebTokensTests
{
    [Fact]
    public void GenerateJwtToken_ReturnsValidJwtToken()
    {
        // Arrange
        IList<Claim> claims = new List<Claim>
        {
            new Claim("claimType1", "claimValue1"),
            new Claim("claimType2", "claimValue2"),
        };
        string jwtSecret = "your_jwt_secret";
        string issuer = "your_issuer";
        string audience = "your_audience";
        string subject = "your_subject";

        TokenHandlerMock tokenHandlerMock = new();

        TokenValidationParametersMock tokenValidationParamsMock = new();
        tokenHandlerMock.MockWriteToken("mocked_jwt_token");

        IJsonWebTokens sut = new JsonWebTokens(tokenHandlerMock.Object, tokenValidationParamsMock.Object);

        // Act
        string jwtToken = sut.GenerateJwtToken(claims, jwtSecret, issuer, audience, subject);

        // Assert
        Assert.Equal("mocked_jwt_token", jwtToken);
        tokenHandlerMock.Verify(th => th.WriteToken(It.IsAny<JwtSecurityToken>()), Times.Once);
    }
    [Fact]
    public void IsValid_ValidToken_ReturnsTrue()
    {
        // Arrange
        var token = "valid_token";

        var tokenHandlerMock = new TokenHandlerMock();
        var tokenValidationParams = new TokenValidationParametersMock();
        var sut = new JsonWebTokens(tokenHandlerMock.Object, tokenValidationParams.Object);

        tokenHandlerMock.MockValidateToken(token);

        // Act
        var result = sut.IsValid(token);

        // Assert
        Assert.True(result);
        tokenHandlerMock.Verify();
    }
    [Fact]
    public void IsValid_InvalidToken_ReturnsFalse()
    {
        // Arrange
        var token = "invalid_token";

        var tokenHandlerMock = new TokenHandlerMock();
        var tokenValidationParams = new TokenValidationParametersMock();
        var sut = new JsonWebTokens(tokenHandlerMock.Object, tokenValidationParams.Object);

        tokenHandlerMock.MockValidateTokenThrows(token);

        // Act
        var result = sut.IsValid(token);

        // Assert
        Assert.False(result);
        tokenHandlerMock.Verify();
    }
}