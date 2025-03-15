namespace Modello.Foundation.AspNetCore.Tests;

public class ResultExtensionsTests
{
    private readonly TestController _controller;

    public ResultExtensionsTests()
    {
        _controller = new TestController
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Request = { Path = "/test" },
                    TraceIdentifier = "trace-id"
                }
            }
        };
    }

    [Fact]
    public void GivenOkResult_WhenToActionResultCalled_ThenReturnsOkResult()
    {
        // Given
        var result = new Mock<IResult>();
        result.Setup(r => r.Status).Returns(ResultStatus.Ok);
        result.Setup(r => r.GetValue()).Returns("test value");

        // When
        var actionResult = ResultExtensions.ToActionResult(result.Object, _controller);

        // Then
        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal("test value", okResult.Value);
    }

    [Fact]
    public void GivenCreatedResult_WhenToActionResultCalled_ThenReturnsCreatedResult()
    {
        // Given
        var result = new Mock<IResult>();
        result.Setup(r => r.Status).Returns(ResultStatus.Created);
        result.Setup(r => r.GetValue()).Returns("test value");
        result.Setup(r => r.Location).Returns("test location");

        // When
        var actionResult = ResultExtensions.ToActionResult(result.Object, _controller);

        // Then
        var createdResult = Assert.IsType<CreatedResult>(actionResult);
        Assert.Equal("test value", createdResult.Value);
        Assert.Equal("test location", createdResult.Location);
    }

    [Fact]
    public void GivenNoContentResult_WhenToActionResultCalled_ThenReturnsNoContentResult()
    {
        // Given
        var result = new Mock<IResult>();
        result.Setup(r => r.Status).Returns(ResultStatus.NoContent);

        // When
        var actionResult = ResultExtensions.ToActionResult(result.Object, _controller);

        // Then
        Assert.IsType<NoContentResult>(actionResult);
    }

    [Fact]
    public void GivenErrorResult_WhenToActionResultCalled_ThenReturnsBadRequestResult()
    {
        // Given
        var error = new ValidationError("Type1", "Error1", "Detail1");
        var result = new Mock<IResult>();
        result.Setup(r => r.Status).Returns(ResultStatus.Error);
        result.Setup(r => r.Errors).Returns([error]);

        // When
        var actionResult = ResultExtensions.ToActionResult(result.Object, _controller);

        // Then
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult);
        var errorResponse = Assert.IsType<ErrorListResponse>(badRequestResult.Value);
        Assert.Equal("/test", errorResponse.Instance);
        Assert.Equal("trace-id", errorResponse.TraceId);
        Assert.Single(errorResponse.Errors);
        Assert.Equal("Type1", errorResponse.Errors.Single().Type);
        Assert.Equal("Error1", errorResponse.Errors.Single().Error);
        Assert.Equal("Detail1", errorResponse.Errors.Single().Detail);
    }

    [Fact]
    public void GivenNotFoundResult_WhenToActionResultCalled_ThenReturnsNotFoundResult()
    {
        // Given
        var result = new Mock<IResult>();
        result.Setup(r => r.Status).Returns(ResultStatus.NotFound);

        // When
        var actionResult = ResultExtensions.ToActionResult(result.Object, _controller);

        // Then
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult);
        var notFoundResponse = Assert.IsType<ErrorResponse>(notFoundResult.Value);
        Assert.Equal("/test", notFoundResponse.Instance);
        Assert.Equal("trace-id", notFoundResponse.TraceId);
        Assert.Equal("NotFound", notFoundResponse.Type);
        Assert.Equal("Not Found", notFoundResponse.Error);
        Assert.Equal("The requested resource was not found.", notFoundResponse.Detail);
    }

    [Fact]
    public void GivenNonExistentResult_WhenToActionResultCalled_ThenThrowsNotSupportedException()
    {
        // Given
        var result = new Mock<IResult>();
        result.Setup(r => r.Status).Returns((ResultStatus)99);

        // When & Then
        Assert.Throws<NotSupportedException>(() => ResultExtensions.ToActionResult(result.Object, _controller));
    }

    private class TestController : ControllerBase { }
}
