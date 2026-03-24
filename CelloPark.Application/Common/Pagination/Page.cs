namespace CelloPark.Application.Common.Pagination;

public sealed record class Page<TSource>(
    IReadOnlyCollection<TSource> Items,
    int Index,
    int Size,
    int Offset,
    int TotalPages,
    int TotalResults,
    bool HasPrevious,
    bool HasNext)
    where TSource : notnull;

public static class Page
{
    public static Page<TSource> Create<TSource>(IReadOnlyCollection<TSource> items, int index, int size, int totalResults)
        where TSource : notnull
    {
        int offset = index * size;
        int totalPages = (int)Math.Ceiling((double)totalResults / size);
        bool hasPrevious = index > 0 && index < totalResults;
        bool hasNext = index < totalPages - 1;

        return new Page<TSource>(items, index, size, offset, totalPages, totalResults, hasPrevious, hasNext);
    }
}
