namespace Modello.Foundation.Tests;

public class PagedResultTests
{
    [Fact]
    public void GivenValidParameters_WhenInitialize_ThenPropertiesAreSet()
    {
        // Given
        var pagedInfo = new PagedInfo(1, 10, 100, 10);
        var value = new List<string>() { "test value" };

        // When
        var pagedResult = new PagedResult<string>(pagedInfo, value);

        // Then
        Assert.Equal(pagedInfo, pagedResult.PagedInfo);
        Assert.Equal(value, pagedResult.Value);
    }
}
