using Microsoft.EntityFrameworkCore;

namespace CelloPark.Infrastructure.Common.Interceptors.Abstractions;

internal interface IUpdateDetailsInterceptor
{
    void ModifyUpdateDetails(DbContext dbContext);
}
