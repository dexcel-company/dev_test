using Microsoft.EntityFrameworkCore;

namespace CelloPark.Infrastructure.Common.Interceptors.Abstractions;

internal interface IShadowIdInterceptor
{
    void ModifyShadowId(DbContext dbContext);
}
