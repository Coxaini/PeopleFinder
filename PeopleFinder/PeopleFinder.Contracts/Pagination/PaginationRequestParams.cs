namespace PeopleFinder.Contracts.Pagination;

public record PaginationRequestParams(int PageNumber = 1, int PageSize = 10);