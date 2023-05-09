namespace PeopleFinder.Domain.Common.Pagination.Cursor;

public class CursorPaginationParams<TC> where TC : struct, IComparable<TC>
{
    
    public CursorPaginationParams(int maxPageSize)
    {
        _maxPageSize = maxPageSize;
    }

    private readonly int _maxPageSize;
    
    private readonly int _pageSize = 10;
    
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value > _maxPageSize ? _maxPageSize : value;
    }
    public TC? After { get; init; }
    public TC? Before { get; init; }
    
}