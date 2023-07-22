namespace ChristopherBriddock.JwtTokens.Tests;

public class JsonWebTokensTests
{
    private readonly JsonWebTokens _jsonWebTokens;
    private readonly string _email = "christopherbriddock@gmail.com";
    private readonly string _jwtSecret = "veryStrongKeyUsedForSigningJWTs123456789";
    private readonly string _issuer = "https://auth.example.com";
    private readonly string _audience = "https://api.example.com";
    private readonly int _expires = 120;
    private readonly string _subject = "John Doe";

    public JsonWebTokensTests()
    {
        _jsonWebTokens = new JsonWebTokens();
    }

    [Fact]
    public async Task TryCreateTokenAsync_ShouldCreateToken_WhenValidParametersAreProvided()
    {
        // Act
        var sut = await _jsonWebTokens.TryCreateTokenAsync(_email,
                                                           _jwtSecret,
                                                           _issuer,
                                                           _audience,
                                                           _expires,
                                                           _subject);

        // Assert
        Assert.True(sut.Success);
        Assert.NotNull(sut.Token);
        Assert.Null(sut.Error);
    }
    [Fact]
    public async Task TryValidateTokenAsync_ShouldValidateToken_WhenValidTokenIsProvided()
    {
        // Arrange
        var sut = new JsonWebTokens();
        var jwtResult = await sut.TryCreateTokenAsync(_email,
                                                      _jwtSecret,
                                                      _issuer,
                                                      _audience,
                                                      _expires,
                                                      _subject);
        var validToken = jwtResult.Token;

        // Act
        var result = await sut.TryValidateTokenAsync(validToken,
                                                     _jwtSecret,
                                                     _issuer,
                                                     _audience);

        // Assert
        Assert.True(result.Success);
        Assert.Null(result.Error);
        Assert.Equal(validToken, result.Token);
    }
    [Fact]
    public async Task TryValidateTokenAsync_ShouldFail_WhenInvalidTokenIsProvided()
    {
        // Arrange
        var sut = new JsonWebTokens();
        var invalidToken = "invalidToken";

        // Act
        var result = await sut.TryValidateTokenAsync(invalidToken, _jwtSecret, _issuer, _audience);

        // Assert
        Assert.False(result.Success);
    }





}
