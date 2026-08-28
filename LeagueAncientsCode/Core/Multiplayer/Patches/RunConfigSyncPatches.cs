using BaseLib.Abstracts;
using HarmonyLib;
using LeagueAncients.Logging;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Runs;

namespace LeagueAncients.Core.Multiplayer.Patches;

/// <summary>
/// Sync the configs of all Clients by broadcasting the Hosts config and storing it as the run-specific-config by everyone else<br/>
/// <para>
/// Run generation reads these settings from <see cref="ActModel.GenerateRooms"/>, which is reached from
/// <see cref="RunManager.GenerateRooms"/> inside <see cref="RunManager.SetUpNewMultiplayer"/>.<br/>
/// On the client that happens after <see cref="StartRunLobby.BeginRunLocally"/> and after the character-select fade-out,
/// so there is a comfortable window between the host publishing its settings and the client needing them.
/// </para>
/// </summary>
[HarmonyPatch]
internal static class RunConfigSyncPatches
{
    /// <summary>
    /// If we are Host or Singleplayer, push our own config settings into the snapshot. (and broadcast to clients if we are the Host)
    /// <para>
    /// Runs before the base method sends LobbyBeginRunMessage.<br/>
    /// Both messages travel NetTransferMode.Reliable, which is reliable-ordered, so every client is guaranteed to process the config
    /// before it reaches BeginRunLocally.
    /// </para>
    /// </summary>
    [HarmonyPatch(typeof(StartRunLobby), "BeginRunForAllPlayers")]
    [HarmonyPrefix]
    private static void PublishHostConfig(StartRunLobby __instance, bool ____isBeginningRun)
    {
        // Vanilla ignores a second call, so do we
        if (____isBeginningRun) return;

        var netService = __instance.NetService;
        if (netService.Type is not (NetGameType.Singleplayer or NetGameType.Host)) return;

        // we are host or singleplayer, so just use our own
        var snapshot = RunConfigSnapshot.FromLocal();
        RunConfigSnapshot.SetActive(snapshot, RunConfigSource.Local);

        if (netService.Type != NetGameType.Host) return;
        ModLog.Info($"Sending run config to clients: {snapshot}");
        CustomMessageWrapper.Send(new HostRunConfigMessage { Snapshot = snapshot }, netService);
    }

    /// <summary>
    /// Client: last chance to notice that the host's settings never arrived.
    /// <para>
    /// On a reliable channel this should not happen. It can happen for multiple reasons.<br/>
    /// In some cases there is nothing to receive and no way to ask. Mod defaults are the least surprising guess;<br/>
    /// Set <see cref="ABORT_RUN_ON_MISSING_HOST_CONFIG"/> to true to refuse the run.
    /// </para>
    /// </summary>
    [HarmonyPatch(typeof(StartRunLobby), "HandleLobbyBeginRunMessage")]
    [HarmonyPrefix]
    private static void EnsureHostConfigApplied(StartRunLobby __instance)
    {
        if (__instance.NetService.Type != NetGameType.Client) return; // only Clients care
        if (RunConfigSnapshot.Source == RunConfigSource.RemoteHost) return; // If we don't have a "RemoteHost" source, something went wrong.

        ModLog.Error(
            "Run is starting but the host never sent its LeagueAncients settings. Falling back to mod defaults. If the host's settings " +
            "differ, this run will desync during map generation!");
        
        if (ABORT_RUN_ON_MISSING_HOST_CONFIG)
        {
            ModLog.Warn("Force disconnecting due to the above error.");
            __instance.NetService.Disconnect(NetError.InternalError);
            return;
        }

        ModLog.Warn("Loading defaults as fallback. This has a high chance to cause desyncs later in the run!");
        RunConfigSnapshot.SetActive(RunConfigSnapshot.Defaults(), RunConfigSource.Fallback);
    }

    // TODO:
    // Currently force disconnects. Once enough Telemetry has been gathered, pick a final option that will yield the highest success rate.
    // If set to false, must somehow let players know this happened.
    
    // TODO:
    // Might be worth not loading default but local settings. Or as mentioned above, use Telemetry to find highest success rate combination.
    // Loading Local allows players who consistently run into this issue to manually sync their configs by mirroring their local settings.
    // This is a huge reason to go into that direction instead of forcing a specific config!
    
    // We start with true to gauge failure rate early in deployment. See above TODOs for additional info.
    
    /// <summary>
    /// If true, a client leaves the lobby rather than starting a run whose generation settings it could not confirm.<br/>
    /// A guessed-but-usually-correct run beats a hard disconnect.
    /// </summary>
    private const bool ABORT_RUN_ON_MISSING_HOST_CONFIG = true; // Usually have consts at the top but this one currently has so much comment baggage I leave it down here.
}
