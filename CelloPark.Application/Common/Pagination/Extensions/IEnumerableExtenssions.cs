using CelloPark.Application.Common.Pagination.Constants;

namespace CelloPark.Application.Common.Pagination.Extensions;

public static class IEnumerableExtenssions
{
    public static Page<TSource> ApplyPagination<TSource>(
        this IEnumerable<TSource> source,
        PaginationCriteria paginationCriteria)
        where TSource : notnull
    {
        int maxSize = PaginationSettings.DefaultMaxSize;

        return source.ApplyPagination(paginationCriteria, maxSize);
    }

    public static Page<TSource> ApplyPagination<TSource>(
        this IEnumerable<TSource> source,
        PaginationCriteria paginationCriteria,
        int maxSize)
        where TSource : notnull
    {
        (int index, int size) = PaginationProvider.AdjustPaginationParameters(paginationCriteria.Index, paginationCriteria.Size, maxSize);
        int count = source.Count();

        List<TSource> items = source
            .Skip(index * size)
            .Take(size)
            .ToList();

        return Page.Create(items, index, size, count);
    }
}
