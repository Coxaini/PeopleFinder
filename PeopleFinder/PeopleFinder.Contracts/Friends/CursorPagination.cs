using System.ComponentModel.DataAnnotations;

namespace PeopleFinder.Contracts.Friends;

public record CursorPagination<T>(int PageSize = 10, T? After = null, T? Before = null) where T : struct;