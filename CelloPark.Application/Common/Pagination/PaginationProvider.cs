using CelloPark.Application.Common.Pagination.Constants;

namespace CelloPark.Application.Common.Pagination;

internal static class PaginationProvider
{
    public static (int index, int size) AdjustPaginationParameters(int? index, int? size, int maxSize)
    {
        maxSize = maxSize < PaginationSettings.DefaultMaxSize ? PaginationSettings.DefaultMaxSize : maxSize;

        index = index is null ? PaginationSettings.DefaultIndex : index;
        size = size is null ? PaginationSettings.DefaultSize : size;

        index = index.Value < 0 ? PaginationSettings.DefaultIndex : index;
        size = size.Value < 0 ? PaginationSettings.DefaultSize : size > maxSize ? maxSize : size;

        return (index.Value, size.Value);
    }
}
