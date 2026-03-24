using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Users.Commands.SignOut.Abstractions;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Users;
using CelloPark.Domain.Features.Users.Entities.RefreshSessions;
using CelloPark.Domain.Features.Users.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Users.Commands.SignOut;

internal sealed class SignOutUserCommandHandler :
    ISignOutUserCommandHandler
{
    public SignOutUserCommandHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<None>> HandleAsync(
        SignOutUserCommand request, CancellationToken cancellationToken = default)
    {
        User? user = await _managementContext.Users
            .Include(user => user.RefreshSessions)
            .FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            return UserErrors.Unauthorized;
        }

        RefreshSession? refreshSession = user.RefreshSessions
            .FirstOrDefault(refreshSession => refreshSession.RefreshToken == request.RefreshToken);

        if (refreshSession is null)
        {
            return UserErrors.Unauthorized;
        }

        ErrorOr<None> removeResult = user.RemoveRefreshSession(refreshSession);

        if (removeResult.IsError)
        {
            return removeResult.Errors;
        }

        await _managementContext.SaveChangesAsync(cancellationToken);

        return None.Value;
    }
}
