using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Ninject.Modules;

namespace ChristopherBriddock.JwtTokens.Console;
public class JsonWebTokensModule : NinjectModule
{
    public override void Load()
    {
        Bind<JwtSecurityTokenHandler>().ToSelf().InSingletonScope();
        // Bind TokenValidationParameters as per your specific implementation
        Bind<TokenValidationParameters>().ToMethod(context =>
        {
            // Configure and return an instance of TokenValidationParameters
            // based on your specific requirements
            return new TokenValidationParameters();
        }).InSingletonScope();

        Bind<IJsonWebTokens>().To<JsonWebTokens>().InSingletonScope();
    }

}
