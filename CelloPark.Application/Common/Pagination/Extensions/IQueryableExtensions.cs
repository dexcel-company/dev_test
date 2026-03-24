using CelloPark.Application.Common.Pagination.Constants;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CelloPark.Application.Common.Pagination.Extensions;

public static class IQueryableExtensions
{
    public static async Task<Page<TSource>> ApplyPaginationAsync<TSource>(
        this IQueryable<TSource> source,
        PaginationCriteria paginationCriteria,
        CancellationToken cancellationToken = default)
        where TSource : notnull
    {
        int maxSize = PaginationSettings.DefaultMaxSize;

        return await ApplyPaginationAsync(source, paginationCriteria, maxSize, cancellationToken);
    }

    public static async Task<Page<TSource>> ApplyPaginationAsync<TSource>(
        this IQueryable<TSource> source,
        PaginationCriteria paginationCriteria,
        int maxSize,
        CancellationToken cancellationToken = default)
        where TSource : notnull
    {
        (int index, int size) = PaginationProvider.AdjustPaginationParameters(paginationCriteria.Index, paginationCriteria.Size, maxSize);
        int count = await source.CountAsync(cancellationToken);
        List<TSource> items = new(size);

        ConfiguredCancelableAsyncEnumerable<TSource> configuredSource = source
            .Skip(index * size)
            .Take(size)
            .AsAsyncEnumerable()
            .WithCancellation(cancellationToken);

        await foreach (TSource item in configuredSource)
        {
            items.Add(item);
        }

        return Page.Create(items, index, items.Count, count);
    }
}
