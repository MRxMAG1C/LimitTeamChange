using System.Text.Json;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace LimitTeamChange;

public class LimitTeamChangePlugin : BasePlugin
{
    public override string ModuleName        => "LimitTeamChange";
    public override string ModuleVersion     => "1.0";
    public override string ModuleAuthor      => "MRxMAG1C";
    public override string ModuleDescription => "Limits how many times a player can switch teams per round.";

    private LimitTeamChangeConfig _config = new();

    // Tracks how many times each player has switched teams this round (keyed by SteamID)
    private readonly Dictionary<ulong, int> _switchCount = new();

    public override void Load(bool hotReload)
    {
        _config = LoadConfig();
        AddCommandListener("jointeam", OnJoinTeam);
        RegisterEventHandler<EventRoundStart>(OnRoundStart);
        Log.Info("Plugin loaded.");
    }

    private HookResult OnJoinTeam(CCSPlayerController? player, CommandInfo _)
    {
        if (player is not { IsValid: true }) return HookResult.Continue;

        // Only count as a switch if the player is already on a real team (CT or T),
        // so that the initial team selection at game join is not penalised.
        if (player.TeamNum is not (2 or 3)) return HookResult.Continue;

        _switchCount.TryGetValue(player.SteamID, out var count);

        if (count >= _config.MaxSwitchesPerRound)
        {
            player.PrintToChat($" {ChatColors.Red}[LimitTeamChange]{ChatColors.White} You can only switch teams {_config.MaxSwitchesPerRound} time(s) per round.");
            Log.Info($"{player.PlayerName} was blocked from switching teams (already switched {count} time(s) this round).");
            return HookResult.Stop;
        }

        _switchCount[player.SteamID] = count + 1;
        Log.Info($"{player.PlayerName} switched teams ({count + 1}/{_config.MaxSwitchesPerRound} this round).");
        return HookResult.Continue;
    }

    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo _)
    {
        _switchCount.Clear();
        return HookResult.Continue;
    }

    private LimitTeamChangeConfig LoadConfig()
    {
        var configPath = Path.Combine(ModuleDirectory, "config.json");

        try
        {
            if (!File.Exists(configPath))
            {
                var defaultConfig = new LimitTeamChangeConfig();
                File.WriteAllText(configPath, JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true }));
                return defaultConfig;
            }

            return JsonSerializer.Deserialize<LimitTeamChangeConfig>(File.ReadAllText(configPath)) ?? new LimitTeamChangeConfig();
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to load config, using defaults: {ex.Message}");
            return new LimitTeamChangeConfig();
        }
    }
}
