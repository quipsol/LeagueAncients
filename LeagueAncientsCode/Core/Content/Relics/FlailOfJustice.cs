using BaseLib.Utils;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace LeagueAncients.Core.Content.Relics;

/// <summary>
/// If you break Block, apply Debuffs to the target.
/// </summary>
[Pool(typeof(EventRelicPool))]
public class FlailOfJustice : LeagueAncientsRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(2), new PowerVar<VulnerablePower>(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<VulnerablePower>()];


    public override async Task AfterBlockBroken(PlayerChoiceContext choiceContext, Creature target, Creature? breaker)
    {
        if (!target.IsMonster || target.IsDead)
            return;
        Flash();
        await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), target, DynamicVars[nameof(WeakPower)].BaseValue, Owner.Creature, null);
        await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), target, DynamicVars[nameof(VulnerablePower)].BaseValue, Owner.Creature, null);

    }

   
}