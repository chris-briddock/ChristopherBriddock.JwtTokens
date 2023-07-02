using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestPlatform;
using Moq;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChristopherBriddock.JwtTokens.Tests.Mocks
{
    public sealed class TokenHandlerMock : Mock<JwtSecurityTokenHandler>
    {
        public void MockWriteToken(string mockedToken)
        {
             Setup(th => th.WriteToken(It.IsAny<JwtSecurityToken>()))
            .Returns(mockedToken);
        }
        public void MockValidateToken(string mockedToken)
        {
            Setup(th => th.ValidateToken(mockedToken,
                                        It.IsAny<TokenValidationParameters>(),
                                        out It.Ref<SecurityToken>.IsAny))
           .Verifiable();



        }
        public void MockValidateTokenThrows(string mockedToken)
        {
            Setup(th => th.ValidateToken(mockedToken,
                                         It.IsAny<TokenValidationParameters>(),
                                         out It.Ref<SecurityToken>.IsAny))
            .Throws<SecurityTokenException>();
        }
    }

}
