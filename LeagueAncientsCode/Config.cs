using BaseLib.Config;

namespace LeagueAncients;

[ConfigHoverTipsByDefault]
public class Config : SimpleModConfig
{
    [ConfigSection("Content")]
    public static bool DisableBaseGameAncients { get; set; } = false;
    public static bool DisableLeagueAncients { get; set; } = false;
}