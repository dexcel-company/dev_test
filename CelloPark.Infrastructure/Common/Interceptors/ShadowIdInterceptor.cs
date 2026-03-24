using CelloPark.Domain.Common.ValueObjects.Abstractions;
using CelloPark.Infrastructure.Common.Interceptors.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CelloPark.Infrastructure.Common.Interceptors;

internal sealed class ShadowIdInterceptor :
    SaveChangesInterceptor, IShadowIdInterceptor
{
    public ShadowIdInterceptor(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    private readonly TimeProvider _timeProvider;

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null)
        {
            return result;
        }

        ModifyShadowId(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return result;
        }

        ModifyShadowId(eventData.Context);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void ModifyShadowId(DbContext dbContext)
    {
        IEnumerable<EntityEntry<IShadowIdOwner>> entities = dbContext.ChangeTracker
            .Entries<IShadowIdOwner>();

        foreach (EntityEntry<IShadowIdOwner> entry in entities)
        {
            if ((entry.State is EntityState.Added or EntityState.Modified)
                && entry.Entity.ShadowId == long.MinValue)
            {
                long ticks = _timeProvider.GetUtcNow().Ticks;

                entry.Property(property => property.ShadowId).CurrentValue = ticks;
            }
        }
    }
}
