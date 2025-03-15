namespace Modello.Foundation.Tests;

public class IResultExtensionsTests
{
    [Fact]
    public void GivenSuccessResult_WhenIsOkCalled_ThenReturnsTrue()
    {
        // Given
        var result = Result.Success(string.Empty);

        // When
        var isOk = result.IsOk();

        // Then
        Assert.True(isOk);
    }

    [Fact]
    public void GivenCreatedResult_WhenIsCreatedCalled_ThenReturnsTrue()
    {
        // Given
        var result = Result.Created(string.Empty, string.Empty);

        // When
        var isCreated = result.IsCreated();

        // Then
        Assert.True(isCreated);
    }

    [Fact]
    public void GivenNoContentResult_WhenIsNoContentCalled_ThenReturnsTrue()
    {
        // Given
        var result = Result.NoContent();

        // When
        var isNoContent = result.IsNoContent();

        // Then
        Assert.True(isNoContent);
    }

    [Fact]
    public void GivenErrorResult_WhenIsErrorCalled_ThenReturnsTrue()
    {
        // Given
        var result = Result.Error();

        // When
        var isError = result.IsError();

        // Then
        Assert.True(isError);
    }

    [Fact]
    public void GivenNotFoundResult_WhenIsNotFoundCalled_ThenReturnsTrue()
    {
        // Given
        var result = Result.NotFound();

        // When
        var isNotFound = result.IsNotFound();

        // Then
        Assert.True(isNotFound);
    }
}
