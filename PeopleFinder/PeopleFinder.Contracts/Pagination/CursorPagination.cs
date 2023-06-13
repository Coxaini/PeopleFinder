namespace PeopleFinder.Contracts.Pagination;

public record CursorPagination<T>(int PageSize = 10, T? After = null, T? Before = null) where T : struct;