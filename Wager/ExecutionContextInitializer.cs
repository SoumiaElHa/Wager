namespace Wager;

public class ExecutionContextInitializer(IExecutionContext executionContext, IHttpContextAccessor httpContextAccessor) : IExecutionContextInitializer
{
    private readonly IExecutionContext _executionContext = executionContext;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private const string PlayerId = "PlayerId";

    public void Init()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return;
        }
        var playerId = Guid.NewGuid();

        if (!_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(PlayerId))
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(PlayerId, playerId.ToString());
            _executionContext.PlayerId = playerId;
        }
        else _executionContext.PlayerId = Guid.TryParse(_httpContextAccessor.HttpContext.Request.Cookies[PlayerId], out playerId) ? playerId : Guid.Empty;
    }
}