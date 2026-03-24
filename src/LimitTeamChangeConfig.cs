using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace LimitTeamChange;

public class LimitTeamChangeConfig : BasePluginConfig
{
    // How many times a player is allowed to switch teams per round.
    [JsonPropertyName("max_switches_per_round")]
    public int MaxSwitchesPerRound { get; set; } = 1;
}
