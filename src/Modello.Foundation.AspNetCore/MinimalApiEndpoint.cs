using Microsoft.AspNetCore.Routing;

namespace Modello.Foundation.AspNetCore;

public abstract class MinimalApiEndpoint
{
    public abstract void Define(IEndpointRouteBuilder builder);
}
