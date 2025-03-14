namespace Modello.Foundation;

public interface IQuery<out TResponse> : IRequest<TResponse>;
