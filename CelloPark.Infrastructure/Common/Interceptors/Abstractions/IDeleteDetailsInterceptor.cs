using Microsoft.EntityFrameworkCore;

namespace CelloPark.Infrastructure.Common.Interceptors.Abstractions;

internal interface IDeleteDetailsInterceptor
{
    void ModifyDeleteDetails(DbContext dbContext);
}
