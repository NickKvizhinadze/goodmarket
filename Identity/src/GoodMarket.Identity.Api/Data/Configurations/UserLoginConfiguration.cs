using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodMarket.Identity.Api.Data.Configurations;

internal class UserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        builder.ToTable(name: "UserLogins");
        builder.HasKey(key => new { key.ProviderKey, key.LoginProvider });
    }
}