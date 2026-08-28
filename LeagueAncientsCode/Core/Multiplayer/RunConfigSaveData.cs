using BaseLib.Patches.Saves;
using HarmonyLib;
using LeagueAncients.Logging;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace LeagueAncients.Core.Multiplayer;


// TODO: Might merge this into class RunConfigSnapshot
// Sidenote: I need to use internal classes more. They exist, use them!
/// <summary>
/// Takes care of syncing anywhere it has to happen during a run.
/// </summary>
[HarmonyPatch]
internal static class RunConfigSaveData
{
    private const string SAVE_ID = MainFile.MOD_ID + ".RunConfig";


    /// <summary>
    /// Register save data through BaseLib.
    /// </summary>
    internal static void Register()
    {
        ExtendedSaveTypes.RegisterObjectSaveType<RunConfigSnapshot>(
            ExtendedSaveTypes.PropertyFunc<RunConfigSnapshot, bool>(nameof(RunConfigSnapshot.DisableBaseGameAncients)),
            ExtendedSaveTypes.PropertyFunc<RunConfigSnapshot, bool>(nameof(RunConfigSnapshot.DisableLeagueAncients))
            );

        ExtendedSaveHandlers<IRunState, SerializableRun>.RegisterSave<RunConfigSnapshot>(
            SAVE_ID,
            _ => RunConfigSnapshot.Active,
            (_, snapshot) =>
            {
                if (snapshot is null) return;
                RunConfigSnapshot.SetActive(snapshot, RunConfigSource.SavedRun);
            });

        ModLog.Info($"Registered extended run save data '{SAVE_ID}'.");
    }

    /// <summary>
    /// A run is over, so the compendium and the next run should use this machine's own settings again.
    /// </summary>
    [HarmonyPatch(typeof(RunManager), nameof(RunManager.CleanUp))]
    [HarmonyPostfix]
    private static void ResetAfterRun() => RunConfigSnapshot.Reset();
}
