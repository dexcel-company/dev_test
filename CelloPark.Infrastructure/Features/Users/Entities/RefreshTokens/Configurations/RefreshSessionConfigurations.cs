using CelloPark.Domain.Features.Users.Entities.RefreshSessions;
using CelloPark.Domain.Features.Users.Entities.RefreshSessions.Constants;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CelloPark.Infrastructure.Features.Users.Entities.RefreshTokens.Configurations;

internal sealed class RefreshSessionConfigurations :
    IEntityTypeConfiguration<RefreshSession>
{
    public void Configure(EntityTypeBuilder<RefreshSession> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.RefreshSessions)
            .HasKey(refreshSession => refreshSession.Id);

        builder
            .Property(refreshSession => refreshSession.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();

        builder
            .Property(refreshSession => refreshSession.RefreshToken)
            .HasMaxLength(RefreshSessionSettings.RefreshTokenMaxLength);

        builder
            .Property(refreshSession => refreshSession.UserAgent)
            .HasMaxLength(RefreshSessionSettings.UserAgentMaxLength);

        builder
            .Property(refreshSession => refreshSession.Fingerprint)
            .HasMaxLength(RefreshSessionSettings.FingerPrintMaxLength);

        builder
            .Property(refreshSession => refreshSession.IpAddress)
            .HasMaxLength(RefreshSessionSettings.IpAddressMaxLength);

        builder
            .Property(refreshSession => refreshSession.CreatedAt)
            .HasConversion(DatabaseContextConverters.DateTimeConverter);

        builder
            .Property(refreshSession => refreshSession.ExpiresIn)
            .HasConversion(DatabaseContextConverters.DateTimeConverter);
    }
}
