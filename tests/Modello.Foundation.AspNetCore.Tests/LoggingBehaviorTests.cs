using Modello.Foundation.AspNetCore.Tests.TestHelpers;

namespace Modello.Foundation.AspNetCore.Tests;

public class LoggingBehaviorTests
{
    private readonly Mock<ILogger<Mediator>> _loggerMock = new();
    private readonly Mock<RequestHandlerDelegate<int>> _delegateMock = new();
    private readonly LoggingBehavior<ConcreteRequest, int> _behavior;

    public LoggingBehaviorTests()
    {
        _behavior = new LoggingBehavior<ConcreteRequest, int>(_loggerMock.Object);
    }

    [Fact]
    public async Task GivenRequest_WhenHandleCalled_ThenLogsInformation()
    {
        // Given
        var request = new ConcreteRequest();
        _delegateMock.Setup(n => n()).ReturnsAsync(1);

        // When
        var response = await _behavior.Handle(request, _delegateMock.Object, CancellationToken.None);

        // Then
        Assert.Equal(1, response);

        _delegateMock.Verify(handler => handler(), Times.Once);
        _loggerMock.VerifyLog(LogLevel.Information, $"Handling {nameof(ConcreteRequest)}", Times.Once);
        _loggerMock.VerifyLog(LogLevel.Information, $"Handled {nameof(ConcreteRequest)}", Times.Once);
    }

    [Fact]
    public async Task GivenRequestWithException_WhenHandleCalled_ThenLogsError()
    {
        // Given
        var request = new ConcreteRequest();
        _delegateMock.Setup(n => n()).ThrowsAsync(new Exception("Exception"));

        // When & Then
        await Assert.ThrowsAsync<Exception>(() => _behavior.Handle(request, _delegateMock.Object, CancellationToken.None));

        _delegateMock.Verify(handler => handler(), Times.Once);
        _loggerMock.VerifyLog(LogLevel.Information, $"Handling {nameof(ConcreteRequest)}", Times.Once);
        _loggerMock.VerifyLog(LogLevel.Error, $"Error while handling {nameof(ConcreteRequest)}", Times.Once);
    }

    private record ConcreteRequest : IRequest<int>;
}
