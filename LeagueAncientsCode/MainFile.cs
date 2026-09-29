using System.Reflection;
using BaseLib.Config;
using BaseLib.Patches.Saves;
using Godot;
using HarmonyLib;
using LeagueAncients.Core.Multiplayer;
using LeagueAncients.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace LeagueAncients;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string MOD_ID = "LeagueAncients"; //Used for resource filepath
    public const string RES_PATH = $"res://{MOD_ID}";
    public static void Initialize()
    {
        ModLog.Info("Begin init");
        
        // Godot
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());
        
        // Config
        ModConfigRegistry.Register(MOD_ID, new Config());
        RunConfigSaveData.Register();

        // Harmony Patching
        Harmony harmony = new(MOD_ID);
        harmony.PatchAll();
        
        ModLog.Info("Adding SavedProperty support for List<SerializableRelic> through BaseLib.");
        ExtendedSaveTypes.RegisterListSaveType<SerializableRelic>();
        ModLog.Warn("This is a warning", LogTopic.Network | LogTopic.Audio);
        ModLog.Error("This is an error", LogTopic.All);
        
        ModLog.Info("Init complete");
        
    }
}