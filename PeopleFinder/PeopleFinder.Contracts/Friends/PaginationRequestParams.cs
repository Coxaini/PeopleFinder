namespace PeopleFinder.Contracts.Friends;

public record PaginationRequestParams(int PageNumber = 1, int PageSize = 10);