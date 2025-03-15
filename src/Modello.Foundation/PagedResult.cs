namespace Modello.Foundation;

public class PagedResult<TValue>(PagedInfo pagedInfo, IEnumerable<TValue>? value) : Result<IEnumerable<TValue>>(value)
{
    public PagedInfo PagedInfo { get; } = pagedInfo;
}
