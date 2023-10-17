using ChristopherBriddock.JwtTokens;
using ChristopherBriddock.JwtTokens.Console;
using Ninject;

IKernel kernel = new StandardKernel();
kernel.Load(new JsonWebTokensModule());

// Resolve an instance of the JsonWebTokens class
var jsonWebTokens = kernel.Get<IJsonWebTokens>();

string secret = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz";
string issuer = "TestIssuer";
string audience = "TestAudience";
string subject = "TestSubject";

string token = (await jsonWebTokens.TryCreateTokenAsync("myemail@email.com",
                                                        secret,
                                                        issuer,
                                                        audience,
                                                        "60",
                                                        subject)).Token;
bool isValid = (await jsonWebTokens.TryValidateTokenAsync(token,
                                                          secret,
                                                          issuer,
                                                          audience)).Success;

Console.WriteLine(isValid);
Console.WriteLine(token);

Console.ReadKey();


