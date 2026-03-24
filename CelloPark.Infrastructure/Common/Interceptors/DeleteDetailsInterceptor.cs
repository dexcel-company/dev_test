using CelloPark.Application.Features.Users.ActionContexts.Abstractions;
using CelloPark.Domain.Common.Enums.Abstractions;
using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Common.ValueObjects.Abstractions;
using CelloPark.Infrastructure.Common.Interceptors.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CelloPark.Infrastructure.Common.Interceptors;

internal sealed class DeleteDetailsInterceptor :
    SaveChangesInterceptor, IDeleteDetailsInterceptor
{
    public DeleteDetailsInterceptor(IUserActionContext userActionContext, TimeProvider timeProvider)
    {
        _userActionContext = userActionContext;
        _timeProvider = timeProvider;
    }

    private readonly IUserActionContext _userActionContext;
    private readonly TimeProvider _timeProvider;

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null)
        {
            return result;
        }

        ModifyDeleteDetails(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return result;
        }

        ModifyDeleteDetails(eventData.Context);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void ModifyDeleteDetails(DbContext dbContext)
    {
        IEnumerable<EntityEntry<IDeleteDetailsOwner>> entities = dbContext.ChangeTracker
            .Entries<IDeleteDetailsOwner>();

        foreach (EntityEntry<IDeleteDetailsOwner> entry in entities)
        {
            if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is IStatusOwner statusOwner && statusOwner.Status == Status.Deleted)
                {
                    DateTimeOffset utcNow = _timeProvider.GetUtcNow();
                    _ = entry.Entity.AddDeleteDetails(utcNow.UtcDateTime, _userActionContext.UserId);
                }
            }
        }
    }
}
