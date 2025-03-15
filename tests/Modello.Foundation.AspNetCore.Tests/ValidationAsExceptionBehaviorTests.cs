namespace Modello.Foundation.AspNetCore.Tests;

public class ValidationAsExceptionBehaviorTests
{
    private readonly Mock<RequestHandlerDelegate<int>> _delegateMock = new();

    public ValidationAsExceptionBehaviorTests()
    {
        _delegateMock.Setup(n => n()).ReturnsAsync(1);
    }

    [Fact]
    public async Task GivenRequestWithNoValidators_WhenHandleCalled_ThenSucceed()
    {
        // Given
        var request = new ConcreteRequest(string.Empty);
        var validators = new List<IValidator<ConcreteRequest>>();
        var behavior = new ValidationAsExceptionBehavior<ConcreteRequest, int>(validators);

        // When
        var response = await behavior.Handle(request, _delegateMock.Object, CancellationToken.None);

        // Then
        Assert.Equal(1, response);

        _delegateMock.Verify(n => n(), Times.Once);
    }

    [Fact]
    public async Task GivenRequestWithSuccessValidators_WhenHandleCalled_ThenSucceed()
    {
        // Given
        var request = new ConcreteRequest("Name");
        var validators = new List<IValidator<ConcreteRequest>> { new ConcreteRequestValidator() };
        var behavior = new ValidationAsExceptionBehavior<ConcreteRequest, int>(validators);

        // When
        var response = await behavior.Handle(request, _delegateMock.Object, CancellationToken.None);

        // Then
        Assert.Equal(1, response);

        _delegateMock.Verify(n => n(), Times.Once);
    }

    [Fact]
    public async Task GivenRequestWithFailValidators_WhenHandleCalled_ThenThrowsException()
    {
        // Given
        var request = new ConcreteRequest(string.Empty);
        var validators = new List<IValidator<ConcreteRequest>> { new ConcreteRequestValidator() };
        var behavior = new ValidationAsExceptionBehavior<ConcreteRequest, int>(validators);

        // When & Then
        await Assert.ThrowsAsync<ValidationException>(() => behavior.Handle(request, _delegateMock.Object, CancellationToken.None));

        _delegateMock.Verify(n => n(), Times.Never);
    }

    private record ConcreteRequest(string Name) : IRequest<int>;

    private class ConcreteRequestValidator : AbstractValidator<ConcreteRequest>
    {
        public ConcreteRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
