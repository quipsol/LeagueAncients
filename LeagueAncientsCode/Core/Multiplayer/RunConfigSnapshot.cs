using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace LeagueAncients.Core.Multiplayer;

/// <summary>
/// Where <see cref="RunConfigSnapshot.Active"/> came from.<br/>
/// Mostly diagnostic, but the client-side run-start check relies
/// on <see cref="RemoteHost"/> to verify the host's settings actually arrived.
/// </summary>
public enum RunConfigSource
{
    /// Should only ever be set if _active is currently null
    None,
    /// A Singleplayer/Host run using their own config settings.
    Local,
    /// Received from the multiplayer host immediately before the run started.
    RemoteHost,
    /// Restored from a run save. Also covers joining a loaded run and rejoining a run in progress,
    /// since both ship a full SerializableRun to the client.
    SavedRun,
    /// Host settings were expected but never arrived, so mod defaults are being used instead.
    Fallback,
}

/// <summary>
/// Gameplay code must read <see cref="Active"/> rather than <see cref="LeagueAncients.Config"/> directly.<br/>
/// In multiplayer the host's snapshot is what every client uses, so that run generation stays
/// identical across the lobby<br/>
/// In singleplayer this also stops a mid-run settings change from altering
/// generation on the next act.
/// </summary>
public sealed class RunConfigSnapshot : IPacketSerializable
{
    public bool DisableBaseGameAncients { get; set; }
    public bool DisableLeagueAncients { get; set; }

    private static RunConfigSnapshot? _active;

    /// <summary>
    /// The settings that gameplay code should obey right now.<br/>
    /// Falls back to the local settings, through lazy loading.
    /// </summary>
    public static RunConfigSnapshot Active => _active ??= FromLocal();

    public static RunConfigSource Source { get; private set; } = RunConfigSource.None;

    /// <summary>
    /// Loads the local config settings.
    /// </summary>
    public static RunConfigSnapshot FromLocal() => new()
    {
        DisableBaseGameAncients = Config.DisableBaseGameAncients,
        DisableLeagueAncients = Config.DisableLeagueAncients,
    };

    /// <summary>
    /// The values a freshly installed copy of the mod would use.<br/>
    /// Used as a last resort when a host's settings were expected but never arrived.
    /// </summary>
    public static RunConfigSnapshot Defaults()
    {
        MainFile.Logger.Warn("Default Config loaded. Something went wrong during Config syncing, the run will likely reach state divergence at some point!");
        // Theoretically they should all be false but who knows what I end up doing, so we grab them like this instead.
        var config = BaseLib.Config.ModConfigRegistry.Get<Config>();
        return new RunConfigSnapshot
        {
            DisableBaseGameAncients = config?.GetDefaultValue<bool>(nameof(Config.DisableBaseGameAncients)) ?? false,
            DisableLeagueAncients = config?.GetDefaultValue<bool>(nameof(Config.DisableLeagueAncients)) ?? false,
        };
    }

    public static void SetActive(RunConfigSnapshot snapshot, RunConfigSource source)
    {
        if(_active is not null)
            MainFile.Logger.Warn("Setting a new run config while one is already set.");
        _active = snapshot;
        Source = source;
        MainFile.Logger.Info($"Active run config ({source}): {snapshot}");
    }

    /// <summary>
    /// Drops any run-scoped snapshot so the local settings apply again. Called when a lobby opens and
    /// when a run is torn down.
    /// </summary>
    public static void Reset()
    {
        if (_active == null && Source == RunConfigSource.Local) return;

        _active = null;
        Source = RunConfigSource.None;
        MainFile.Logger.Info("Active run config reset to null.");
    }

    public void Serialize(PacketWriter writer)
    {
        writer.WriteBool(DisableBaseGameAncients);
        writer.WriteBool(DisableLeagueAncients);
    }

    public void Deserialize(PacketReader reader)
    {
        DisableBaseGameAncients = reader.ReadBool();
        DisableLeagueAncients = reader.ReadBool();
    }

    public override string ToString() => $"{nameof(DisableBaseGameAncients)}={DisableBaseGameAncients}, {nameof(DisableLeagueAncients)}={DisableLeagueAncients}";
}
