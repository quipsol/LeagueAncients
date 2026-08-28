using System.Reflection;
using HarmonyLib;
using LeagueAncients.Core.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
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
    
    // private static IEnumerable<MethodBase> TargetMethods()
    // {
    //     return AccessTools
    //                 .GetTypesFromAssembly(typeof(ActModel).Assembly)
    //                 .Where(type => typeof(ActModel).IsAssignableFrom(type))
    //                 .Select(type => AccessTools.DeclaredMethod(type, nameof(ActModel.GetUnlockedAncients), [typeof(UnlockState)]))
    //                 .Where(methodInfo => methodInfo != null && !methodInfo.IsAbstract);
    // }
    //
    // [HarmonyPostfix]
    // private static void RemoveBaseGameAncients(ref IEnumerable<AncientEventModel> __result)
    // {
    //     // This removes them from the Compendium too. And I cant check if RunState is null because you can open the Compendium inside runs.
    //     // Do I have to transpile ActModel.GenerateRooms?
    //     if (RunManager.Instance.DebugOnlyGetState() is null) return; // not a fix :(
    //     if(RunConfigSnapshot.Active.DisableBaseGameAncients && !RunConfigSnapshot.Active.DisableLeagueAncients)
    //         __result = __result.Except(ManualListOfBaseAncients);
    // }
    
    // This makes an extra Rng call and I dont like that
    [HarmonyPatch(typeof(ActModel), nameof(ActModel.GenerateRooms))]
    [HarmonyPostfix]
    private static void RemoveBaseGameAncients(ActModel __instance, ref RoomSet ____rooms, List<AncientEventModel>? ____sharedAncientSubset, Rng rng, UnlockState unlockState)
    {
        if(RunConfigSnapshot.Active.DisableBaseGameAncients && !RunConfigSnapshot.Active.DisableLeagueAncients)
            ____rooms.Ancient = rng.NextItem(__instance.GetUnlockedAncients(unlockState).Except(ManualListOfBaseAncients).Concat(____sharedAncientSubset ?? []))!;
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