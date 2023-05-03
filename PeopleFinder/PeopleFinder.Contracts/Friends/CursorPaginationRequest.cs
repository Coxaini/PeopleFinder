using System.ComponentModel.DataAnnotations;

namespace PeopleFinder.Contracts.Friends;

public record CursorPaginationRequest<T>([Range(1, 20)]int PageSize = 10, T? After = null, T? Before = null) where T : struct;