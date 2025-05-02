namespace Wager;

public interface IExecutionContext
{
    /// <summary>
    /// Current player identifier
    /// </summary>
    Guid PlayerId { get; set; }
}