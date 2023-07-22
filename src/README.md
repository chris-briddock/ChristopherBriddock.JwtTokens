# ChristopherBriddock.JwtTokens

This repository contains a simple library for managing JSON Web Tokens (JWTs) in ASP.NET Core applications. It includes methods for creating, validating JWTs and a middleware for injecting JWTs into the request context.

## Table of Contents

- [Getting Started](#getting-started)
- [Usage](#usage)
- [Testing](#testing)


## Getting Started

### Dependencies

This library relies on the following packages:

- System.IdentityModel.Tokens.Jwt
- Microsoft.IdentityModel.Tokens
- Microsoft.Extensions.Configuration

### Installation

This can be installed as a NuGet package [here](https://www.nuget.org/packages/ChristopherBriddock.AspNetCore.Extensions/)
Or you can download or fork this and customize to your liking.

## Usage

### Adding JWT Middleware to ASP.NET Core Pipeline

To use the `JwtMiddleware` in an ASP.NET Core application, register it in the `Configure` method of the `Startup.cs` class:

```csharp
public class Startup
{
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // other middleware registrations...
        
        // Add the JWT Middleware
        app.UseMiddleware<JwtMiddleware>();
        
        // other middleware registrations...
    }
}

## Testing

This library has been designed to be testable, and unit tests can be written using xUnit and Moq 

To run the existing unit tests use 'dotnet test'
