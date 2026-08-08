using System.Reflection;
using Godot;
using HarmonyLib;
using LeagueAncients.ArtRoller;
using MegaCrit.Sts2.Core.Modding;

namespace LeagueAncients;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string MOD_ID = "LeagueAncients"; //Used for resource filepath
    public const string RES_PATH = $"res://{MOD_ID}";
    
    public static readonly string CardsDirectory = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                "ArtRoller");
    
    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(MOD_ID, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Directory.CreateDirectory(CardsDirectory);
        CardArtRoller.RegisterAllFromDirectory(CardsDirectory);
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());

        Harmony harmony = new(MOD_ID);

        harmony.PatchAll();
    }
}