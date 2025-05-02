namespace Wager;

public class ExecutionContextMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Creates new ExecutionContextMiddleware
    /// </summary>
    /// <param name="next">Next delegate</param>
    public ExecutionContextMiddleware(RequestDelegate next) => _next = next;

    /// <summary>
    /// Request pipeline handler
    /// </summary>
    /// <param name="httpContext">HttpContext</param>
    /// <param name="executionContextInitializer">Execution context middleware</param>
    public async Task InvokeAsync(
        HttpContext httpContext,
        IExecutionContextInitializer executionContextInitializer)
    {
        executionContextInitializer.Init();

        await _next(httpContext);
    }
}

