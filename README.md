# LimitTeamChange
A [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp) plugin that limits how many times a player can switch teams per round.

## Features
- Blocks team switches once a player has reached the configured limit
- Notifies the player in chat when they are blocked from switching
- Initial team selection at game join is not counted as a switch
- Switch counter resets automatically at the start of each round

## Configuration
A `config.json` file is automatically created in `configs/plugins/LimitTeamChange/` on first load.

| Field | Description |
|---|---|
| `max_switches_per_round` | Number of team switches allowed per player per round. Defaults to `1`. |

## Version
1.0

## Author
MRxMAG1C
