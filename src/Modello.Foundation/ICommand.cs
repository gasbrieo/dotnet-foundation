namespace Modello.Foundation;

public interface ICommand<out TResponse> : IRequest<TResponse>;
