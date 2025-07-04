# GoodMarket.Identity.Api

This service provides authentication and authorization for the GoodMarket microservices ecosystem using IdentityServer and JWT Bearer tokens.

## Features

- User registration and management
- JWT Bearer authentication
- Centralized authorization for all client APIs

---

## How to Integrate Authorization in Client APIs

To secure your API with IdentityServer, follow these steps:

### 1. Add Required NuGet Packages

Ensure your client API project references the following packages:

- Microsoft.AspNetCore.Authentication.JwtBearer
- IdentityServer4.AccessTokenValidation

You can install them via NuGet Package Manager or CLI:

```sh
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package IdentityServer4.AccessTokenValidation -v 4.1.2
```

---

### 2. Configure Authentication and Authorization in `Program.cs`

Add the following code to your `Program.cs` or `Startup.cs`:

```csharp
builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.ApiName = "api"; // Must match the API resource name configured in IdentityServer
        options.Authority = "https://identity.goodmarket.ge"; // URL of the Identity API
        // options.RequireHttpsMetadata = false; // Uncomment for development if not using HTTPS
    });

builder.Services.AddAuthorization();
```

---

### 3. Add Middleware in the HTTP Pipeline

Ensure the following lines are present in your middleware configuration (after `app.UseRouting()` and before `app.UseEndpoints()`):

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

---

### 4. Secure Your Endpoints

Use the `[Authorize]` attribute to protect controllers or actions:

```csharp
[Authorize]
[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
    // Your actions here
}
```

---

### 5. Testing

- Obtain a valid JWT access token from the Identity API (e.g., via login endpoint).
- Include the token in the `Authorization` header of your requests:

```
Authorization: Bearer {access_token}
```

---

## Notes

- The `ApiName` in the authentication options must match the API resource name registered in IdentityServer.
- The `Authority` should point to the Identity API’s base URL.
- For development, you may need to disable HTTPS metadata validation by setting `options.RequireHttpsMetadata = false;`.

---

## Troubleshooting

- **401 Unauthorized:** Ensure the access token is valid and not expired. Check that the `ApiName` and `Authority` are correct.
- **CORS Issues:** Make sure CORS is configured to allow requests from your client applications.

---

## Further Reading

- [IdentityServer4 Documentation](https://identityserver4.readthedocs.io/)
- [Microsoft Docs: Authentication and Authorization](https://docs.microsoft.com/en-us/aspnet/core/security/)

---

Feel free to customize this README for your specific environment and deployment details.
