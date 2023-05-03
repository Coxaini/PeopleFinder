namespace PeopleFinder.Domain.Common.Pagination.Cursor;

public class CursorPaginationParams<TC> where TC : struct, IComparable<TC>
{
    private const int MaxPageSize = 10;
    
    private int _pageSize = 10;
    
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
    public TC? After { get; set; }
    public TC? Before { get; set; }
    
}