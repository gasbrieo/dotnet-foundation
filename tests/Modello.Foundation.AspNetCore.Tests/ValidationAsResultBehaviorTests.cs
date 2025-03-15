namespace Modello.Foundation.AspNetCore.Tests;

public class ValidationAsResultBehaviorTests
{
    private readonly Mock<RequestHandlerDelegate<Result<int>>> _delegateMock = new();

    public ValidationAsResultBehaviorTests()
    {
        _delegateMock.Setup(n => n()).ReturnsAsync(1);
    }

    [Fact]
    public async Task GivenRequestWithNoValidators_WhenHandleCalled_ThenReturnsSuccess()
    {
        // Given
        var request = new ConcreteRequest(string.Empty);
        var validators = new List<IValidator<ConcreteRequest>>();
        var behavior = new ValidationAsResultBehavior<ConcreteRequest, Result<int>>(validators);

        // When
        var response = await behavior.Handle(request, _delegateMock.Object, CancellationToken.None);

        // Then
        Assert.Equal(1, response.GetValue());

        _delegateMock.Verify(n => n(), Times.Once);
    }

    [Fact]
    public async Task GivenRequestWithSuccessValidators_WhenHandleCalled_ThenReturnsSuccess()
    {
        // Given
        var request = new ConcreteRequest("Name");
        var validators = new List<IValidator<ConcreteRequest>> { new ConcreteRequestValidator() };
        var behavior = new ValidationAsResultBehavior<ConcreteRequest, Result<int>>(validators);

        // When
        var response = await behavior.Handle(request, _delegateMock.Object, CancellationToken.None);

        // Then
        Assert.Equal(1, response.GetValue());

        _delegateMock.Verify(n => n(), Times.Once);
    }

    [Fact]
    public async Task GivenRequestWithFailValidators_WhenHandleCalled_ThenReturnsError()
    {
        // Given
        var request = new ConcreteRequest(string.Empty);
        var validators = new List<IValidator<ConcreteRequest>> { new ConcreteRequestValidator() };
        var behavior = new ValidationAsResultBehavior<ConcreteRequest, Result<int>>(validators);

        // When
        var response = await behavior.Handle(request, _delegateMock.Object, CancellationToken.None);

        // Then
        Assert.Equal(ResultStatus.Error, response.Status);
        Assert.Single(response.Errors);

        Assert.Equal("ValidationError", response.Errors.Single().Type);
        Assert.Equal("NotEmptyValidator", response.Errors.Single().Error);
        Assert.Equal("'Name' must not be empty.", response.Errors.Single().Detail);

        _delegateMock.Verify(n => n(), Times.Never);
    }

    private record ConcreteRequest(string Name) : IRequest<Result<int>>;

    private class ConcreteRequestValidator : AbstractValidator<ConcreteRequest>
    {
        public ConcreteRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
