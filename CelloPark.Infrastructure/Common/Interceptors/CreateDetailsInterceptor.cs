using CelloPark.Application.Features.Users.ActionContexts.Abstractions;
using CelloPark.Domain.Common.ValueObjects.Abstractions;
using CelloPark.Infrastructure.Common.Interceptors.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CelloPark.Infrastructure.Common.Interceptors;

internal sealed class CreateDetailsInterceptor :
    SaveChangesInterceptor, ICreateDetailsInterceptor
{
    public CreateDetailsInterceptor(IUserActionContext userActionContext, TimeProvider timeProvider)
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

        ModifyCreateDetails(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return result;
        }

        ModifyCreateDetails(eventData.Context);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void ModifyCreateDetails(DbContext dbContext)
    {
        IEnumerable<EntityEntry<ICreateDetailsOwner>> entities = dbContext.ChangeTracker
            .Entries<ICreateDetailsOwner>();

        foreach (EntityEntry<ICreateDetailsOwner> entry in entities)
        {
            if (entry.State == EntityState.Added)
            {
                DateTimeOffset utcNow = _timeProvider.GetUtcNow();
                _ = entry.Entity.AddCreateDetails(utcNow.UtcDateTime, _userActionContext.UserId);
            }
        }
    }
}
