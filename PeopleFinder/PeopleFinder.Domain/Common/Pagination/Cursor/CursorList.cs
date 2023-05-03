using System.Collections;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace PeopleFinder.Domain.Common.Pagination.Cursor;

public class CursorList<T> where T : class
{
    public CursorList(IList<T> items,int limit , int totalCount)
    {
        TotalCount = totalCount;
        if (items.Count <= totalCount && items.Count == limit+1)
        {
            Next = items.Last();
            items.RemoveAt(items.Count - 1);
        }
        
        Items = items.ToList();
    }

    public CursorList()
    {
        Items = new List<T>();
    }
    public int TotalCount { get; }
    public T? Next { get; }
    public IReadOnlyList<T> Items { get; }

}
