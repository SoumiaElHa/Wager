using System.Text.Json.Serialization;

namespace Wager.Data;

public class BetResultDto
{
    /// <summary>
    /// The player total account
    /// </summary>
    public long Account { get; set; }

    /// <summary>
    /// The number of points lost or gained
    /// </summary>
    public long Points { get; set; }

    /// <summary>
    /// The result of the bet (won or lost)
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ResultStatus Status { get; set; }
}