using System.Diagnostics.CodeAnalysis;
using MegaCrit.Sts2.Core.Runs;

namespace LeagueAncients.Util;

/// <summary>
/// Helper class that wraps various undesirable actions
/// </summary>
public static class GameHelper
{
    /// <summary>
    /// Try to get the currently active IRunState object.
    /// </summary>
    /// <param name="state">the <c>IRunState</c> object</param>
    /// <returns><c>true</c> if a RunState is active</returns>
    public static bool TryGetRunState([NotNullWhen(true)]out IRunState? state)
    {
        RunState? statee = new PrivatePropertyWrapper<RunManager, RunState>(RunManager.Instance, "State").Value;
        state = RunManager.Instance?.DebugOnlyGetState() ?? null;
        return state != null;
    }

    /// Returns the currently active IRunState object if it exists
    public static IRunState? GetNullableRunState() => RunManager.Instance?.DebugOnlyGetState() ?? null;
    /// Returns the currently active IRunState object if it exists, otherwise throw a NRE
    public static IRunState GetRunState() => GetNullableRunState() ?? throw new NullReferenceException();
}