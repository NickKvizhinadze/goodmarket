using Microsoft.AspNetCore.Identity;

namespace GoodMarket.Identity.Api.Domain.Users;

public class ApplicationUser: IdentityUser
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    public ICollection<IdentityRole> Roles { get; init; } = [];
    public ICollection<IdentityUserRole<string>> UserRoles { get; init; } = [];
}