namespace LeagueAncients.Logging;

[Flags]
public enum LogTopic
{
    None = 0,
    Default = 1 << 0,
    Input = 1 << 1,
    Audio = 1 << 2,
    Visual = 1 << 3,
    GameState = 1 << 4,
    Network = 1 << 5,
    Model = 1 << 6,
    All = ~0
}