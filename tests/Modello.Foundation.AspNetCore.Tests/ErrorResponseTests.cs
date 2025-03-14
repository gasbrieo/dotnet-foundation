namespace Modello.Foundation.AspNetCore.Tests;

public class ErrorResponseTests
{
    [Fact]
    public void GivenValidParameters_WhenInitialize_ThenPropertiesAreSet()
    {
        // Given
        var instance = "/test";
        var traceId = "trace-id";
        var type = "ErrorType";
        var error = "ErrorMessage";
        var detail = "ErrorDetail";

        // When
        var errorResponse = new ErrorResponse(instance, traceId, type, error, detail);

        // Then
        Assert.Equal(instance, errorResponse.Instance);
        Assert.Equal(traceId, errorResponse.TraceId);
        Assert.Equal(type, errorResponse.Type);
        Assert.Equal(error, errorResponse.Error);
        Assert.Equal(detail, errorResponse.Detail);
    }
}
