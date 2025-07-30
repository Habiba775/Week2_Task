using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using System.Security.Claims;
using Duende.IdentityModel;
using Microsoft.AspNetCore.Identity;
using week2_Task.Models;

public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly UserManager<APPUser> _userManager;

    public ResourceOwnerPasswordValidator(UserManager<APPUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var user = await _userManager.FindByEmailAsync(context.UserName);
        if (user != null && await _userManager.CheckPasswordAsync(user, context.Password))
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, user.UserName),
                new Claim(JwtClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
            }

            context.Result = new GrantValidationResult(
                subject: user.Id,
                authenticationMethod: OidcConstants.AuthenticationMethods.Password,
                claims: claims
            );
        }
        else
        {
            context.Result = new GrantValidationResult(
                TokenRequestErrors.InvalidGrant,
                "Invalid username or password"
            );
        }
    }
}




