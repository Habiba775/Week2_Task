using Duende.IdentityModel;
using Duende.IdentityServer.Models;

namespace week2_Task
{

    public static class Config
    {
        
        public static IEnumerable<ApiScope> ApiScopes =>
    new List<ApiScope>
    {
        new ApiScope("myapi", "My API", new[] {
            JwtClaimTypes.Name,
            JwtClaimTypes.Email,
            JwtClaimTypes.Role
        })
    };

        public static IEnumerable<Client> Clients =>
    new List<Client>
    {
        new Client
        {
            ClientId = "myclient",
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            ClientSecrets = { new Secret("secret".Sha256()) },

            
            AlwaysIncludeUserClaimsInIdToken = true,
            AlwaysSendClientClaims = true,

            AllowedScopes = { "myapi" }
        }
    };
    }
}