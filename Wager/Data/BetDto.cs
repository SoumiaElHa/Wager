namespace Wager.Data;

public class BetDto
{
    /// <summary>
    /// The random number that a player can bet
    /// </summary>
    public short Number { get; set; }

    /// <summary>
    /// The number of points a player bets with
    /// </summary>
    public int Points { get; set; }
}