using Microsoft.EntityFrameworkCore;

namespace CelloPark.Infrastructure.Common.Interceptors.Abstractions;

internal interface ICreateDetailsInterceptor
{
    void ModifyCreateDetails(DbContext dbContext);
}
