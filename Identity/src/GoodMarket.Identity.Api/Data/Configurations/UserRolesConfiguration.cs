using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodMarket.Identity.Api.Data.Configurations;

internal class UserRolesConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.ToTable(name: "UserRoles");
        builder.HasKey(key => new { key.UserId, key.RoleId });
    }
}