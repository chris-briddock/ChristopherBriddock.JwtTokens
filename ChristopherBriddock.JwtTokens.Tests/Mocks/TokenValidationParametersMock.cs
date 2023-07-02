using Microsoft.IdentityModel.Tokens;
using Moq;

namespace ChristopherBriddock.JwtTokens.Tests.Mocks
{
    public sealed class TokenValidationParametersMock : Mock<TokenValidationParameters>
    {
        public void MockClone()
        {
            Setup(st => st.Clone()).Returns(It.IsAny<TokenValidationParameters>());
        }
    }
}
