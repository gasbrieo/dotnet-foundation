namespace Modello.Foundation.AspNetCore;

public record ErrorListResponse(string Instance, string TraceId, IEnumerable<ErrorDetail> Errors);
