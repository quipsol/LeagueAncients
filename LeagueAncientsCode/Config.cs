using BaseLib.Config;

namespace LeagueAncients;

[ConfigHoverTipsByDefault]
public class Config : SimpleModConfig
{
    
    // if both are set, the base game removal will be ignored!
    /// If set to true, base game Ancients will be filtered out of the ancient pool
    [ConfigSection("Content")]
    public static bool DisableBaseGameAncients { get; set; } = false;
    /// If set to true, League Ancients will be filtered out of the pool
    public static bool DisableLeagueAncients { get; set; } = false;
}