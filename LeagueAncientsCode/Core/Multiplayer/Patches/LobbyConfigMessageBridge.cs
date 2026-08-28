using BaseLib.Abstracts;
using HarmonyLib;
using LeagueAncients.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Runs;

namespace LeagueAncients.Core.Multiplayer.Patches;

/// <summary>
/// <para>
/// Makes BaseLib's custom messages usable during the character-select lobby.
/// </para>
/// <para>
/// BaseLib registers its CustomMessageWrapper handler in <see cref="RunManager.InitializeShared"/>, which only
/// runs once a run has already started. Anything sent earlier would hit NetMessageBus with no handler
/// registered for the type and be dropped with an error. So we register a handler of our own on the
/// lobby's INetGameService for the duration of the lobby.
/// </para>
/// <para>
/// The handler deliberately only forwards <see cref="HostRunConfigMessage"/>.<br/>
/// BaseLib's handler owns everything else once the run starts.<br/>
/// The two registrations coexist briefly (InitializeShared runs before the lobby is cleaned up), which
/// is harmless because applying a snapshot is idempotent.
/// </para>
/// <para>
/// If BaseLib ever gains native lobby-phase registration, this whole class can be deleted with (hopefully) no other changes.
/// </para>
/// </summary>
[HarmonyPatch]
internal static class LobbyConfigMessageBridge
{
    /// <summary>
    /// The net service of the lobby we are currently in, or null outside a lobby.
    /// </summary>
    private static INetGameService? _lobbyNetService;

    // Patching the 4-argument constructor covers both overloads
    [HarmonyPatch(typeof(StartRunLobby), MethodType.Constructor,
                typeof(GameMode), typeof(INetGameService), typeof(IStartRunLobbyListener), typeof(int))]
    [HarmonyPostfix]
    private static void RegisterOnLobbyOpened(StartRunLobby __instance)
    {
        RunConfigSnapshot.Reset(); // A stale RemoteHost snapshot must be cleared to prevent false client-side check success.
        Register(__instance.NetService);
    }

    [HarmonyPatch(typeof(StartRunLobby), nameof(StartRunLobby.CleanUp))]
    [HarmonyPrefix]
    private static void UnregisterOnLobbyClosed(StartRunLobby __instance) => Unregister(__instance.NetService);

    
    internal static void ApplyHostConfig(HostRunConfigMessage message, ulong senderId)
    {
        if (_lobbyNetService is not { Type: NetGameType.Client })
        {
            ModLog.Warn($"Ignoring {nameof(HostRunConfigMessage)} from {senderId}: not currently a lobby client.");
            return;
        }

        if (RunConfigSnapshot.Source == RunConfigSource.RemoteHost) return; // already applied this lobby

        RunConfigSnapshot.SetActive(message.Snapshot, RunConfigSource.RemoteHost);
    }

    
    private static void Register(INetGameService netService)
    {
        if (ReferenceEquals(_lobbyNetService, netService)) return;

        Unregister(_lobbyNetService); // Clean up just in case
        netService.RegisterMessageHandler<CustomMessageWrapper>(HandleWrappedMessage);
        _lobbyNetService = netService;
        ModLog.Debug($"Listening for custom messages on the {netService.Type} lobby.");
    }

    private static void Unregister(INetGameService? netService)
    {
        if (netService == null) return;

        netService.UnregisterMessageHandler<CustomMessageWrapper>(HandleWrappedMessage);
        if (ReferenceEquals(_lobbyNetService, netService)) _lobbyNetService = null; // only set to null if it's the one we intended to unregister from
    }

    private static void HandleWrappedMessage(CustomMessageWrapper wrapper, ulong senderId)
    {
        if (wrapper.Message is HostRunConfigMessage) wrapper.Message.HandleMessage(senderId); // Will reroute back to "ApplyHostConfig" above
    }
}
