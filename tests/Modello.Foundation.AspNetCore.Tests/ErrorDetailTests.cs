namespace Modello.Foundation.AspNetCore.Tests;

public class ErrorDetailTests
{
    [Fact]
    public void GivenValidParameters_WhenInitialize_ThenPropertiesAreSet()
    {
        // Given
        var type = "ErrorType";
        var error = "ErrorMessage";
        var detail = "ErrorDetail";

        // When
        var errorDetail = new ErrorDetail(type, error, detail);

        // Then
        Assert.Equal(type, errorDetail.Type);
        Assert.Equal(error, errorDetail.Error);
        Assert.Equal(detail, errorDetail.Detail);
    }
}
