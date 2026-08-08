using BaseLib.Abstracts;
using LeagueAncients.Core.Multiplayer.Patches;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace LeagueAncients.Core.Multiplayer;

/// <summary>
/// Carries the host's <see cref="RunConfigSnapshot"/> to every client, sent from
/// <see cref="RunConfigSyncPatches"/> immediately before the vanilla LobbyBeginRunMessage.
/// </summary>
public sealed class HostRunConfigMessage : ICustomMessage
{
    public RunConfigSnapshot Snapshot = new();

    /// Host to clients only; clients never relay this.
    public bool ShouldBroadcast => false;

    /// <summary>
    /// Must not be buffered. StartRunLobby.BeginRunLocally turns message buffering on, and a buffered
    /// message would only be released once the run has already generated its rooms.
    /// </summary>
    public bool ShouldBuffer => false;

    public void Serialize(PacketWriter writer) => Snapshot.Serialize(writer);

    public void Deserialize(PacketReader reader) => Snapshot.Deserialize(reader);

    /// <summary>
    /// Dispatch lives in <see cref="LobbyConfigMessageBridge"/>, which knows whether we are actually a
    /// lobby client.<br/>
    /// Routing through it keeps this working unchanged if BaseLib ever registers custom
    /// message handlers during the lobby itself.
    /// </summary>
    public void HandleMessage(ulong senderId) => LobbyConfigMessageBridge.ApplyHostConfig(this, senderId);
}
