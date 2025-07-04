using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GoodMarket.Identity.Api.Domain.Users;

namespace GoodMarket.Identity.Api.Data.Configurations;

internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users");
        builder.HasQueryFilter(r => !r.IsDeleted);

        builder.HasMany(e => e.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<string>>
            (au => au.HasOne<IdentityRole>().WithMany().HasForeignKey(u => u.RoleId),
                au => au.HasOne<ApplicationUser>().WithMany(user => user.UserRoles).HasForeignKey(r => r.UserId));

        builder.Navigation(u => u.UserRoles).AutoInclude();
        builder.Navigation(u => u.Roles).AutoInclude();
    }
}