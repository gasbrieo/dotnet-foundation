namespace Modello.Foundation.AspNetCore.Tests;

public class ErrorListResponseTests
{
    [Fact]
    public void GivenValidParameters_WhenInitialize_ThenPropertiesAreSet()
    {
        // Given
        var instance = "/test";
        var traceId = "trace-id";
        var errors = new List<ErrorDetail>
        {
            new("Type1", "Error1", "Detail1"),
            new("Type2", "Error2", "Detail2")
        };

        // When
        var errorListResponse = new ErrorListResponse(instance, traceId, errors);

        // Then
        Assert.Equal(instance, errorListResponse.Instance);
        Assert.Equal(traceId, errorListResponse.TraceId);
        Assert.Equal(errors, errorListResponse.Errors);
    }
}
