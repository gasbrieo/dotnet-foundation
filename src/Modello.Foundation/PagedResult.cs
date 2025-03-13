namespace Modello.Foundation;

public class PagedResult<TValue>(PagedInfo pagedInfo, TValue? value) : Result<TValue>(value)
{
    public PagedInfo PagedInfo { get; } = pagedInfo;
}