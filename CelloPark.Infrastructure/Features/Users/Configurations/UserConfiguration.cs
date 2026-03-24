using CelloPark.Domain.Features.Users;
using CelloPark.Domain.Features.Users.Constants;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CelloPark.Infrastructure.Features.Users.Configurations;

internal sealed class UserConfiguration :
    IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.Users)
            .HasKey(user => user.Id);

        builder
            .Property(user => user.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();

        builder
            .Property(user => user.FirstName)
            .HasMaxLength(UserSettings.FirstNameMaxLength);

        builder
            .Property(user => user.LastName)
            .HasMaxLength(UserSettings.LastNameMaxLength);

        builder
            .Property(user => user.Email)
            .HasMaxLength(UserSettings.EmailMaxLength);

        builder
            .Property(user => user.PhoneNumber)
            .HasMaxLength(UserSettings.PhoneNumberMaxLength);

        builder
            .Property(user => user.JobTitle)
            .HasMaxLength(UserSettings.JobTitleMaxLength);

        builder
            .Property(user => user.Password)
            .HasMaxLength(UserSettings.PasswordMaxLength);

        builder.Ignore(user => user.CreateDetails);

        //ConfigureCreateDetails(builder); TODO

        ConfigureRefreshSession(builder);
    }

    private static void ConfigureCreateDetails(EntityTypeBuilder<User> builder)
    {
        builder
            .OwnsOne(user => user.CreateDetails, buildAction =>
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

    private static void ConfigureRefreshSession(EntityTypeBuilder<User> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(User.RefreshSessions))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(user => user.RefreshSessions)
            .WithOne()
            .HasPrincipalKey(user => user.Id)
            .HasForeignKey(refreshSession => refreshSession.UserId);
    }
}
