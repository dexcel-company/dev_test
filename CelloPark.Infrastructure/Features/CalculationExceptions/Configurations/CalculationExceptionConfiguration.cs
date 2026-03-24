using CelloPark.Domain.Features.CalculationExceptions;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.CalculationExceptions.Configurations;

internal sealed class CalculationExceptionConfiguration :
    IEntityTypeConfiguration<CalculationException>
{
    public void Configure(EntityTypeBuilder<CalculationException> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.CalculationExceptions)
            .HasKey(calculationExceptions => calculationExceptions.Id);

        builder
            .Property(calculationExceptions => calculationExceptions.Id)
            .ValueGeneratedNever();

        builder
            .Property(calculationException => calculationException.DateTime)
            .HasColumnName("ExtractionDate");
    }
}
