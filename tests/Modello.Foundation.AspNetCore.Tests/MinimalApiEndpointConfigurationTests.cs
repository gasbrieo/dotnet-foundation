namespace Modello.Foundation.AspNetCore.Tests;

public class MinimalApiEndpointConfigurationTests
{
    private readonly MinimalApiEndpointConfiguration _configuration = new();

    [Fact]
    public void GivenType_WhenFromTypeCalled_ThenAddsTypeToList()
    {
        // Given

        // When
        _configuration.FromType<ConcreteFirstMinimalApiEndpoint>();

        // Then
        Assert.Single(_configuration.GetEndpoints());
        Assert.Contains(typeof(ConcreteFirstMinimalApiEndpoint), _configuration.GetEndpoints());
    }

    [Fact]
    public void GivenAssembly_WhenFromAssemblyCalled_ThenAddsTypesToList()
    {
        // Given
        var assembly = Assembly.GetExecutingAssembly();

        // When
        _configuration.FromAssembly(assembly);

        // Then
        Assert.Equal(3, _configuration.GetEndpoints().Count());
        Assert.Contains(typeof(ConcreteFirstMinimalApiEndpoint), _configuration.GetEndpoints());
        Assert.Contains(typeof(ConcreteSecondMinimalApiEndpoint), _configuration.GetEndpoints());
        Assert.Contains(typeof(ConcreteThirdMinimalApiEndpoint), _configuration.GetEndpoints());
    }

    private class ConcreteFirstMinimalApiEndpoint : MinimalApiEndpoint
    {
        public override void Define(IEndpointRouteBuilder builder)
        {
        }
    }

    private class ConcreteSecondMinimalApiEndpoint : MinimalApiEndpoint
    {
        public override void Define(IEndpointRouteBuilder builder)
        {
        }
    }

    private class ConcreteThirdMinimalApiEndpoint : MinimalApiEndpoint
    {
        public override void Define(IEndpointRouteBuilder builder)
        {
        }
    }
}

public class MinimalApiResultExtensionsTests
{
    private readonly HttpContext _context = new DefaultHttpContext
    {
        Request = { Path = "/test" },
        TraceIdentifier = "trace-id"
    };

    [Fact]
    public void GivenOkResult_WhenToMinimalApiResultCalled_ThenReturnsOkResult()
    {
        // Given
        var result = new Mock<IResult>();
        result.Setup(r => r.Status).Returns(ResultStatus.Ok);
        result.Setup(r => r.GetValue()).Returns("test value");

        // When
        var minimalApiResult = MinimalApiResultExtensions.ToMinimalApiResult(result.Object, _context);

        // Then
        var okResult = Assert.IsType<Ok<object>>(minimalApiResult);
        Assert.Equal("test value", okResult.Value);
    }

    [Fact]
    public void GivenCreatedResult_WhenToMinimalApiResultCalled_ThenReturnsCreatedResult()
    {
        // Given
        var result = new Mock<IResult>();
        result.Setup(r => r.Status).Returns(ResultStatus.Created);
        result.Setup(r => r.GetValue()).Returns("test value");
        result.Setup(r => r.Location).Returns("test location");

        // When
        var minimalApiResult = MinimalApiResultExtensions.ToMinimalApiResult(result.Object, _context);

        // Then
        var createdResult = Assert.IsType<Created<object>>(minimalApiResult);
        Assert.Equal("test value", createdResult.Value);
        Assert.Equal("test location", createdResult.Location);
    }

    [Fact]
    public void GivenNoContentResult_WhenToMinimalApiResultCalled_ThenReturnsNoContentResult()
    {
        // Given
        var result = new Mock<IResult>();
        result.Setup(r => r.Status).Returns(ResultStatus.NoContent);

        // When
        var minimalApiResult = MinimalApiResultExtensions.ToMinimalApiResult(result.Object, _context);

        // Then
        Assert.IsType<NoContent>(minimalApiResult);
    }

    [Fact]
    public void GivenErrorResult_WhenToMinimalApiResultCalled_ThenReturnsBadRequestResult()
    {
        // Given
        var error = new ValidationError("Type1", "Error1", "Detail1");
        var result = new Mock<IResult>();
        result.Setup(r => r.Status).Returns(ResultStatus.Error);
        result.Setup(r => r.Errors).Returns([error]);

        // When
        var minimalApiResult = MinimalApiResultExtensions.ToMinimalApiResult(result.Object, _context);

        // Then
        var badRequestResult = Assert.IsType<BadRequest<ErrorListResponse>>(minimalApiResult);
        var errorResponse = badRequestResult.Value;
        Assert.NotNull(errorResponse);
        Assert.Equal("/test", errorResponse.Instance);
        Assert.Equal("trace-id", errorResponse.TraceId);
        Assert.Single(errorResponse.Errors);
        Assert.Equal("Type1", errorResponse.Errors.Single().Type);
        Assert.Equal("Error1", errorResponse.Errors.Single().Error);
        Assert.Equal("Detail1", errorResponse.Errors.Single().Detail);
    }

    [Fact]
    public void GivenNotFoundResult_WhenToMinimalApiResultCalled_ThenReturnsNotFoundResult()
    {
        // Given
        var result = new Mock<IResult>();
        result.Setup(r => r.Status).Returns(ResultStatus.NotFound);

        // When
        var minimalApiResult = MinimalApiResultExtensions.ToMinimalApiResult(result.Object, _context);

        // Then
        var notFoundResult = Assert.IsType<NotFound<ErrorResponse>>(minimalApiResult);
        var notFoundResponse = notFoundResult.Value;
        Assert.NotNull(notFoundResponse);
        Assert.Equal("/test", notFoundResponse.Instance);
        Assert.Equal("trace-id", notFoundResponse.TraceId);
        Assert.Equal("NotFound", notFoundResponse.Type);
        Assert.Equal("Not Found", notFoundResponse.Error);
        Assert.Equal("The requested resource was not found.", notFoundResponse.Detail);
    }

    [Fact]
    public void GivenNonExistentResult_WhenToMinimalApiResultCalled_ThenThrowsNotSupportedException()
    {
        // Given
        var result = new Mock<IResult>();
        result.Setup(r => r.Status).Returns((ResultStatus)99);

        // When & Then
        Assert.Throws<NotSupportedException>(() => MinimalApiResultExtensions.ToMinimalApiResult(result.Object, _context));
    }
}
