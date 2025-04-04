namespace Modello.Foundation.AspNetCore;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder builder, Assembly assembly)
    {
        builder.RegisterEndpoints(config => config.FromAssembly(assembly));
    }

    public static void RegisterEndpoints(this IEndpointRouteBuilder builder, Action<MinimalApiEndpointConfiguration> configure)
    {
        var configuration = new MinimalApiEndpointConfiguration();

        configure(configuration);

        foreach (var type in configuration.GetEndpoints())
        {
            if (Activator.CreateInstance(type) is MinimalApiEndpoint endpointInstance)
            {
                endpointInstance.Define(builder);
            }
        }
    }
}
