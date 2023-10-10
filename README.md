# Public Token Authentication in ASP.NET Core

This project explores an example of an ASP.NET Core middleware that provides authentication using a specific "public token" retrieved from the HTTP request headers.

## Contents

- [PublicTokenAuthHandler](#publictokenauthhandler)
- [PublicTokenAuthOptions](#publictokenauthoptions)
- [Installation](#installation)
  
## PublicTokenAuthHandler

The `PublicTokenAuthHandler` class extracts a token from a request, validates this token, and associates it with a particular user. If the token is valid, the user is marked as successfully authenticated.

```csharp
public class PublicTokenAuthHandler : AuthenticationHandler<PublicTokenAuthOptions>
{
    ...
}
```

## PublicTokenAuthOptions
The PublicTokenAuthOptions class holds configuration settings for the authentication scheme, including information about how the token will be defined within headers.

```csharp
public class PublicTokenAuthOptions : AuthenticationSchemeOptions
{
    public const string DefaultSchemeName = "PublicTokenAuthenticationScheme";
    public string TokenHeaderName { get; set; } = "projectToken";
}
```

## Installation
This section should guide through the process of installing or setting up your project for using this authentication handler. For instance, explain how to register and configure middleware in your Program.cs file.

```csharp
builder.Services.AddAuthentication(PublicTokenAuthOptions.DefaultSchemeName)
    .AddScheme<PublicTokenAuthOptions, PublicTokenAuthHandler>(PublicTokenAuthOptions.DefaultSchemeName, opts => { });

...

builder.Services.AddSwaggerGen(c =>
{
    ...
    c.OperationFilter<CustomHeaderSwagger>();
    ...
});
```
