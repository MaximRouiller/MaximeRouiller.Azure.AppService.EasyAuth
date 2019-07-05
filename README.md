# MaximeRouiller.Azure.AppService.EasyAuth

This is a temporary project meant as a workaround for people wanting to use [Azure AppService EasyAuth][EasyAuth].

This provides support for filling up the `ClaimsPrincipal` from an application that is already logged in through [EasyAuth][EasyAuth].

This was tested using .NET Core 2.2.

The authentication provider provided by [EasyAuth][EasyAuth] that were tested were:

* Azure Active Directory
* Microsoft Account
* Google
* Facebook
* Twitter

## How to setup

First, install the NuGet package [`MaximeRouiller.Azure.AppService.EasyAuth`](https://www.nuget.org/packages/MaximeRouiller.Azure.AppService.EasyAuth/). This can be done either directly from Visual Studio or by running a CLI command like the following.

```bash
dotnet add package MaximeRouiller.Azure.AppService.EasyAuth -v 0.1.0-beta82
```

Once the package is installed, make sure that Controllers that require authentication are decorated with the `[Authorize(AuthenticationSchemes = "EasyAuth")]` attribute. This attribute can be set anywhere as long as it uses the right `AuthenticationSchemes`.

### Controller

```csharp
[HttpGet]
[Authorize(AuthenticationSchemes = "EasyAuth")]
public ActionResult<IEnumerable<string>> Get()
{
  ...
}
```

### Startup.cs

Add the following line to your `Startup.cs` file.

```csharp
using MaximeRouiller.Azure.AppService.EasyAuth
// ...
public void ConfigureServices(IServiceCollection services)
{
    //... rest of the file
    services.AddAuthentication().AddEasyAuthAuthentication((o) => { });
}
```

## How does it work

This package provide a custom [`AuthenticationHandler`](https://docs.microsoft.com/dotnet/api/microsoft.aspnetcore.authentication.authenticationhandler-1?view=aspnetcore-2.2&WT.mc_id=easyauth-github-marouill) that will interpret the `X-MS-CLIENT-PRINCIPAL-IDP` and `X-MS-CLIENT-PRINCIPAL` HTTP headers that are sent by [EasyAuth][EasyAuth] once a user is logged in.

Every Controller action with the `Authorize` attribute mentioned previously will go through this custom `AuthenticationHandler`. The handler will base64 decode the `X-MS-CLIENT-PRINCIPAL` and create a new [`ClaimsPrincipal`](https://docs.microsoft.com/dotnet/api/system.security.claims.claimsprincipal?view=netcore-2.2&WT.mc_id=easyauth-github-marouill) with the claims contained within the header.

There is no attempt to validate any tokens of any sort as this is the job of EasyAuth. This package assume that EasyAuth will never forward a malicious `X-MS-CLIENT-PRINCIPAL` and that EasyAuth will never send us an un-authenticated request.

This component will not enable any "challenge" of authentication and only parse the headers sent by EasyAuth. If you want to force the authentication, you can forward your user to `/.auth/login/{provider}`. If you want to automatically redirect your user to certain page, you can add `?post_login_redirect_url=/my-page`.

Reading [Advanced usage of authentication and authorization in Azure App Service](https://docs.microsoft.com/azure/app-service/app-service-authentication-how-to?WT.mc_id=easyauth-github-marouill) will greatly help you understand how to use [EasyAuth][EasyAuth] as well.

## Fine prints

This package is not supported by Microsoft and by using this package, you agree that no support will be provided. Issues can be opened and I will do my best to resolve them. There is no guarantee provided with this package in any sort, kind, nor will there be in the future. While this coded was created while I'm employed at Microsoft, it hasn't gone through security review, code review, etc. 

Use at your own risk.


[EasyAuth]: https://docs.microsoft.com/en-us/azure/app-service/overview-authentication-authorization
[NuGetPackage]: todo