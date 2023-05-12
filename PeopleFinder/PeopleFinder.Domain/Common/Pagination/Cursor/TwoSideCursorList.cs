namespace PeopleFinder.Domain.Common.Pagination.Cursor;

public class TwoSideCursorList<T, TC> : CursorList<T> where T : class where TC : struct
{
    public TwoSideCursorList(IList<T> items,int limit , Func<T,TC> cursorGetter,TC? afterCursor = null,
        TC? beforeCursor = null, int? totalCount= null)
    {
        TotalCount = totalCount;

        if (afterCursor != null && cursorGetter(items.First()).Equals(afterCursor))
        {
            Previous = items.First();
            items.RemoveAt(0); 
        }
        
        if(beforeCursor != null && cursorGetter(items.Last()).Equals(beforeCursor))
        {
            Next = items.Last();
            items.RemoveAt(items.Count - 1);
        }
        
        Items = items.ToList();
    }
    
    public T? Previous { get; init; }
    
}