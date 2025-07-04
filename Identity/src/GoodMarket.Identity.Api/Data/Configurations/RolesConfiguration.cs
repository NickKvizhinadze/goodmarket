using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GoodMarket.Identity.Api.Shared;
using GoodMarket.Identity.Api.Constants;

namespace GoodMarket.Identity.Api.Data.Configurations;

internal class RolesConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.ToTable("Roles");
        builder.HasData(
            new IdentityRole
            {
                Id = DataBaseConstants.AdminRoleId,
                Name = UserRoles.Admin,
                NormalizedName =  UserRoles.Admin.ToUpper(),
                ConcurrencyStamp = DateTimeHelpers.CreateUtcDate(2025, 01, 01).Ticks.ToString()
            },
            new IdentityRole
            {
                Id = DataBaseConstants.CustomerRoleId,
                Name = UserRoles.Customer,
                NormalizedName =  UserRoles.Customer.ToUpper(),
                ConcurrencyStamp = DateTimeHelpers.CreateUtcDate(2025, 01, 01).Ticks.ToString()
            }
        );
    }
}