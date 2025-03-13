namespace Modello.Foundation.Tests;

public class PagedInfoTests
{
    [Fact]
    public void GivenValidParameters_WhenInitialize_ThenPropertiesAreSet()
    {
        // Given
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 100;
        var totalPages = 10;

        // When
        var pagedInfo = new PagedInfo(pageNumber, pageSize, totalItems, totalPages);

        // Then
        Assert.Equal(pageNumber, pagedInfo.PageNumber);
        Assert.Equal(pageSize, pagedInfo.PageSize);
        Assert.Equal(totalItems, pagedInfo.TotalItems);
        Assert.Equal(totalPages, pagedInfo.TotalPages);
    }
}
