using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChristopherBriddock.JwtTokens.Tests.Mocks
{
    internal class TokenHandlerMockForRefreshToken : Mock<JwtSecurityTokenHandler>
    {
        public void MockValidateToken(string token)
        {
            Setup(th => th.ValidateToken(token, It.IsAny<TokenValidationParameters>(), out It.Ref<SecurityToken>.IsAny))
                .Returns(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public void MockValidateTokenThrows(string token)
        {
            Setup(th => th.ValidateToken(token, It.IsAny<TokenValidationParameters>(), out It.Ref<SecurityToken>.IsAny))
                .Throws<SecurityTokenException>();
        }

        public void MockWriteToken(string newToken)
        {
            Setup(th => th.WriteToken(It.IsAny<JwtSecurityToken>()))
                .Returns(newToken);
        }

        public new void Verify()
        {
            Verify(th => th.ValidateToken(It.IsAny<string>(), It.IsAny<TokenValidationParameters>(), out It.Ref<SecurityToken>.IsAny), Times.Once);
            Verify(th => th.WriteToken(It.IsAny<JwtSecurityToken>()), Times.Once);
        }
    }
}
