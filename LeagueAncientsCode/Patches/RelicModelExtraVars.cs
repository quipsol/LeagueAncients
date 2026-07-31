using HarmonyLib;
using LeagueAncients.Util;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LeagueAncients.Patches;


[HarmonyPatch]
public static class RelicModelExtraVars
{
    private static readonly AccessTools.FieldRef<RelicModel, Player?> OwnerRef =
                AccessTools.FieldRefAccess<RelicModel, Player?>("_owner");
    
    [HarmonyPatch(typeof(RelicModel), nameof(RelicModel.DynamicDescription), MethodType.Getter)]
    [HarmonyPostfix]
    public static void AddToDynamicDescription(RelicModel __instance, ref LocString __result)
    {
        __result.Add("IsEvent", false);
        AddVars(ref __result, __instance);
    }
    
    [HarmonyPatch(typeof(RelicModel), nameof(RelicModel.DynamicEventDescription), MethodType.Getter)]
    [HarmonyPostfix]
    public static void AddToDynamicEventDescription(RelicModel __instance, ref LocString __result)
    {
        __result.Add("IsEvent", true);
        AddVars(ref __result, __instance);
    }

    private static void AddVars(ref LocString locString, RelicModel __instance)
    {
        var owner = OwnerRef(__instance);
        var hasOwner = owner is not null;
        locString.Add("HasOwner", hasOwner);
        locString.Add("InRun", hasOwner); // currently there is no need to distinct between IsRun and HasOwner
        locString.Add("IsMultiplayer", hasOwner && owner!.RunState!.Players.Count > 1);
        
        if (!hasOwner) return;
        foreach (var dynamicVar in __instance.DynamicVars.Values.OfType<CalculatedRelicVar>().ToList())
            dynamicVar.UpdatePreviewVar(false);
    }
    
}
