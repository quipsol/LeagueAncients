using System.Reflection;
using HarmonyLib;
using LeagueAncients.Core.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Unlocks;

namespace LeagueAncients.Patches;

[HarmonyPatch]
public static class DisableBaseGameAncients
{
    // Using a manual list here because trying to gather them dynamically might end up including modded Ancients
    // Currently excluding Neow. If I implement Pantheon this needs extra logic because adding Act 1 Ancients is tricky.
    private static List<AncientEventModel> ManualListOfBaseAncients =>
    [
                ModelDb.AncientEvent<Orobas>(),
                ModelDb.AncientEvent<Pael>(),
                ModelDb.AncientEvent<Tezcatara>(),
                ModelDb.AncientEvent<Nonupeipe>(),
                ModelDb.AncientEvent<Tanx>(),
                ModelDb.AncientEvent<Vakuu>(),
                //ModelDb.AncientEvent<Neow>(),
    ];
    
    private static IEnumerable<MethodBase> TargetMethods()
    {
        return AccessTools
                    .GetTypesFromAssembly(typeof(ActModel).Assembly)
                    .Where(type => typeof(ActModel).IsAssignableFrom(type))
                    .Select(type => AccessTools.DeclaredMethod(type, nameof(ActModel.GetUnlockedAncients), [typeof(UnlockState)]))
                    .Where(methodInfo => methodInfo != null && !methodInfo.IsAbstract);
    }
    
    [HarmonyPostfix]
    private static void RemoveBaseGameAncients(ref IEnumerable<AncientEventModel> __result)
    {
        if(RunConfigSnapshot.Active.DisableBaseGameAncients && !RunConfigSnapshot.Active.DisableLeagueAncients)
            __result = __result.Except(ManualListOfBaseAncients);
    }
}

[HarmonyPatch]
public static class DisableBaseGameSharedAncients
{
    // Using a manual list here because trying to gather them dynamically might end up including modded Ancients
    private static List<AncientEventModel> ManualListOfSharedBaseAncients =>
    [
                ModelDb.AncientEvent<Darv>(),
    ];
    
    [HarmonyPatch(typeof(ActModel), nameof(ActModel.SetSharedAncientSubset))]
    [HarmonyPrefix]
    private static void RemoveBaseGameSharedAncients(ref IEnumerable<AncientEventModel> sharedAncientSubset)
    {
        if(RunConfigSnapshot.Active.DisableBaseGameAncients && !RunConfigSnapshot.Active.DisableLeagueAncients)
            sharedAncientSubset = sharedAncientSubset.Except(ManualListOfSharedBaseAncients);
    }
}