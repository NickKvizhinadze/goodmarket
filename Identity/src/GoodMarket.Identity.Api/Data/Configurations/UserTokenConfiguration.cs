using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodMarket.Identity.Api.Data.Configurations;

internal class UserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
    {
        builder.ToTable(name: "UserTokens");
        builder.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });
    }
}