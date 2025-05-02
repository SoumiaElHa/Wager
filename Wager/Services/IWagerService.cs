using Wager.Data;

namespace Wager.Services;

public interface IWagerService
{
    BetResultDto Bet(BetDto bet);
}
