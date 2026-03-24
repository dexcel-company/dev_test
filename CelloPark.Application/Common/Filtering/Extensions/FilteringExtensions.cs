using System.Linq.Expressions;

namespace CelloPark.Application.Common.Filtering.Extensions;

public static class FilteringExtensions
{
    private const string Ascending = "ASC";
    private const string Descending = "DESC";

    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(
        this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, string? sortMethod)
    {
        if (string.IsNullOrEmpty(sortMethod))
        {
            return source.OrderBy(keySelector);
        }

        return sortMethod switch
        {
            _ when string.Equals(Ascending, sortMethod, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(keySelector),
            _ when string.Equals(Descending, sortMethod, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderByDescending(keySelector),
            _ =>
                source.OrderBy(keySelector)
        };
    }
}
