using Microsoft.Extensions.Caching.Memory;
using Wager.Data;

namespace Wager.Services;

public class WagerService(IExecutionContext executionContext, IMemoryCache cache, ILogger<WagerService> logger) : IWagerService
{
    private readonly IExecutionContext _executionContext = executionContext;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<WagerService> _logger = logger;

    public BetResultDto Bet(BetDto bet)
    {
        _logger.LogInformation("The player bet {0} with a number of points {1}", bet.Number, bet.Points);

        if (bet.Number > 9 || bet.Number < 0)
            throw new ArgumentException("Bet number should be between 0 and 9 ", nameof(bet.Number));

        // get the player identifier from the execution context
        var currentPlayer = _executionContext.PlayerId;

        //set the account in the cache if not exists or get it from the cache 
        if (!_cache.TryGetValue(currentPlayer, out long? account))
            _cache.Set(currentPlayer, account = 10000);

        var betResult = new BetResultDto();

        int random = Random.Shared.Next(1, 10);

        if (random == bet.Number)
        {
            betResult.Points = 9 * bet.Points;
            betResult.Status = ResultStatus.Won;
        }
        else betResult.Points = -bet.Points;

        betResult.Account = account!.Value + betResult.Points;
        _cache.Set(currentPlayer, betResult.Account);

        _logger.LogInformation("The player {0} a number of points {1}", betResult.Status.ToString(), betResult.Points);

        return betResult;
    }
}
