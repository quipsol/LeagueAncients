using HarmonyLib;
using LeagueAncients.Extensions;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Models.Potions;

namespace LeagueAncients.Core.AncientPotions;


// TODO:
// While this is funny and understandable to do. I should not change base game rarities.
// I will leave it at Event Rarity in final release and just manually add it to every List I use.
// Annoying but safer for mod compatability.
[HarmonyPatch]
public static class AmbergrisPatch
{
    [HarmonyPatch(typeof(Ambergris), nameof(Ambergris.Rarity), MethodType.Getter)]
    [HarmonyPostfix]
    private static void ChangeToAncient(ref PotionRarity __result)
    {
        __result = PotionRarity.Ancient;
    }
}