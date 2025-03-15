namespace Modello.Foundation.Tests;

public class PagedListTests
{
    [Fact]
    public void GivenValidParameters_WhenInitialize_ThenPropertiesAreSet()
    {
        // Given
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 100;
        var items = new List<string>();

        // When
        var pagedList = new PagedList<string>(pageNumber, pageSize, totalItems, items);

        // Then
        Assert.Equal(pageNumber, pagedList.PageNumber);
        Assert.Equal(pageSize, pagedList.PageSize);
        Assert.Equal(totalItems, pagedList.TotalItems);
        Assert.Equal(10, pagedList.TotalPages);
        Assert.Equal(items, pagedList.Items);
    }
}
