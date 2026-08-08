using System.Reflection;
using System.Reflection.Emit;
using BaseLib.Utils;
using Godot;
using HarmonyLib;
using LeagueAncients.Extensions;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.PotionLab;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Unlocks;

namespace LeagueAncients.Core.AncientPotions;

[HarmonyPatch]
public static class InsertAncientCategoryIntoPotionLab
{
    private static AddedNode<NPotionLab, NPotionLabCategory> _ancientLabCategory = new("res://scenes/screens/potion_lab/potion_category.tscn",
                (nPotionLab, nPotionLabCategory) =>
    {
        var rareNode = nPotionLab.GetNode("%Rare");
        var container = rareNode.GetParent();
        container.AddChildSafely(nPotionLabCategory);
        container.MoveChild(nPotionLabCategory, rareNode.GetIndex() + 1);
    });



    [HarmonyPatch(typeof(NPotionLab), "LoadPotions", MethodType.Async)]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> ReplaceConnectMethodWithCustom(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        // foreach (var instruction in instructions)
        // {
        //     MainFile.Logger.Info($"OpCode: {instruction.opcode} | Operand: {instruction.operand}");
        // }
        
        var a = AccessTools.Method(typeof(InsertAncientCategoryIntoPotionLab), nameof(MyMethod));

        var matcher = new CodeMatcher(instructions)
                    .MatchStartForward(
                    [
                                new CodeMatch(OpCodes.Newobj),
                                new CodeMatch(OpCodes.Stloc_S),
                    ])
                    .ThrowIfInvalid("Could not find creation of List in NPotionLab.LoadPotions")
                    .Advance(1);

        var listOperand = matcher.Instruction.operand;
        matcher.Advance(1)
                    .Insert([
                                
                                new CodeInstruction(OpCodes.Ldloc_1), // Ldarg_0 is the state machine. It stored the actual "this" for NPotionLab in Ldloc_1
                                // we load address because I use ref below. Using ref and not using Ldloca will lead to horrible things!!!! (but no patching error!)
                                // we could just not use ref below since its a list, but I wanted to keep it as an example.
                                new CodeInstruction(OpCodes.Ldloca_S, listOperand), 
                                new CodeInstruction(OpCodes.Call, a)
                    ]);
        
        // MainFile.Logger.Info($"=========================");
        // foreach (var instruction in  matcher.InstructionEnumeration())
        // {
        //     MainFile.Logger.Info($"OpCode: {instruction.opcode} | Operand: {instruction.operand}");
        // }
        
        return matcher.InstructionEnumeration();
    }

    private static void MyMethod(NPotionLab instance, ref List<IReadOnlyList<Control>> controls)
    {
        var unlockState = SaveManager.Instance.GenerateUnlockStateFromProgress();
        var allUnlockedPotions = unlockState.Potions.ToHashSet();
        var seenPotions = SaveManager.Instance.Progress.DiscoveredPotions.Select(ModelDb.GetByIdOrNull<PotionModel>).OfType<PotionModel>().ToHashSet();
        var ancientCategory = _ancientLabCategory.Get(instance);
        ancientCategory.LoadPotions(PotionRarity.Ancient, new LocString("potion_lab", "LEAGUEANCIENTS-ANCIENT"), seenPotions, unlockState, allUnlockedPotions);
        controls.AddRange(ancientCategory.GetGridItems());
    }
}