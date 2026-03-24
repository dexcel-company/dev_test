using CelloPark.Application.Features.Users.Dtos;
using CelloPark.Domain.Features.Users;

namespace CelloPark.Application.Features.Users.Extensions;

public static class UserExtensions
{
    public static UserAuditDto ToAuditDto(this User model)
    {
        return new UserAuditDto
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
        };
    }
}
