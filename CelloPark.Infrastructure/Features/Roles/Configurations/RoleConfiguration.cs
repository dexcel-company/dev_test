using CelloPark.Domain.Features.Roles;
using CelloPark.Domain.Features.Roles.Constants;
using CelloPark.Domain.Features.Users;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CelloPark.Infrastructure.Features.Roles.Configurations;

internal sealed class RoleConfiguration :
    IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.Roles)
            .HasKey(role => role.Id);

        builder
            .Property(role => role.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();

        builder
            .Property(role => role.Name)
            .HasMaxLength(RoleSettings.NameMaxLength);

        ConfigureCreateDetails(builder);

        ConfigureUser(builder);
    }

    private static void ConfigureCreateDetails(EntityTypeBuilder<Role> builder)
    {
        builder
            .OwnsOne(role => role.CreateDetails, buildAction =>
            {
                buildAction
                    .Property(createDetails => createDetails.CreatedAt)
                    .HasConversion(DatabaseContextConverters.DateTimeConverter)
                    .HasColumnName(DatabaseContextColumnNames.CreatedAt);

                buildAction
                    .Property(createDetails => createDetails.CreatedBy)
                    .HasColumnName(DatabaseContextColumnNames.CreatedBy);

                buildAction
                    .HasOne(createDetails => createDetails.User)
                    .WithMany()
                    .HasPrincipalKey(user => user.Id)
                    .HasForeignKey(createDetails => createDetails.CreatedBy);
            });
    }

    private static void ConfigureUser(EntityTypeBuilder<Role> builder)
    {
        builder
            .HasMany<User>()
            .WithOne(user => user.Role)
            .HasPrincipalKey(role => role.Id)
            .HasForeignKey(user => user.RoleId);
    }
}
