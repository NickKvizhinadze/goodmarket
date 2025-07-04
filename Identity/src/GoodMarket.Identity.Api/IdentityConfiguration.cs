using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace GoodMarket.Identity.Api;

public class IdentityConfiguration
{
    public static List<TestUser> TestUsers =>
        new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1144",
                Username = "mukesh",
                Password = "mukesh",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Mukesh Murugan"),
                    new Claim(JwtClaimTypes.GivenName, "Mukesh"),
                    new Claim(JwtClaimTypes.FamilyName, "Murugan"),
                    new Claim(JwtClaimTypes.WebSite, "http://codewithmukesh.com"),
                }
            }
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    ];

    public static IEnumerable<ApiScope> ApiScopes =>
    [
        new("api.read"),
        new("api.write")
    ];
    
    public static IEnumerable<ApiResource> ApiResources =>
    [
        new ApiResource("api")
            {
                Scopes = new List<string>{ "api.read","api.write" },
                ApiSecrets = new List<Secret>{ new Secret("supersecret".Sha256()) } //TODO: generate secret
            }
    ];
    
    public static IEnumerable<Client> Clients =>
    [
        new Client
            {
                ClientId = "cwm.client",
                ClientName = "Client Credentials Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "api.read" }
            }
    ];
}