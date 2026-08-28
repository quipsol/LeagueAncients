using System.Diagnostics;
using BaseLib.Abstracts;
using LeagueAncients.Core.Models;
using LeagueAncients.Logging;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LeagueAncients.Core.Content.Potions;

// Taric
public class BottledStarlight : LeagueAncientsPotionModel
{
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<IntangiblePower>(1)];
    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<IntangiblePower>()];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        if (target is null) return;
        await PowerCmd.Apply<IntangiblePower>(choiceContext, target, DynamicVars[nameof(IntangiblePower)].BaseValue, Owner.Creature, null);
    }
}