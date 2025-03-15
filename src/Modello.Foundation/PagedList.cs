namespace Modello.Foundation;

public class PagedList<TValue>(int pageNumber, int pageSize, int totalItems, IEnumerable<TValue> items)
{
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public int TotalItems { get; } = totalItems;
    public int TotalPages { get; } = (int)Math.Ceiling(totalItems / (double)pageSize);
    public IEnumerable<TValue> Items { get; } = items;
}
