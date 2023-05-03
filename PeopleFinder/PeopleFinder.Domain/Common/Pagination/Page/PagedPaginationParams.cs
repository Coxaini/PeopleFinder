namespace PeopleFinder.Domain.Common.Pagination.Page;

public class PagedPaginationParams
{
    const int MaxPageSize = 20;
    
    private int _pageNumber = 1;
    public int PageNumber { get => _pageNumber; set=> _pageNumber= value <= 0 ? 1 : value;}
    private  int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}