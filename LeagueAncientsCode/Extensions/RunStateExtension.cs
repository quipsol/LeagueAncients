using MegaCrit.Sts2.Core.Runs;

namespace LeagueAncients.Extensions;

public static class RunStateExtension
{
    extension(IRunState runState)
    {
        public bool IsMultiplayer => runState.Players.Count > 1;
    }
}