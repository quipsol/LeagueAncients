using LeagueAncients.Core.Content.Powers;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LeagueAncients.Core.Content.Cards;


public class Burden() : LeagueAncientsCardModel(1, CardType.Skill, CardRarity.Ancient, TargetType.AllAllies)
{
    // public override string BetaPortraitPath => "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";
    // public override string PortraitPath => null!;// "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";
    // public override string? CustomPortraitPath => null;// "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<IntangiblePower>(1), new PowerVar<BurdenPower>(3)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<IntangiblePower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        foreach (var player in Owner.Creature.CombatState!.Allies.Where(c => c is {IsPlayer: true, IsAlive: true}))
            await PowerCmd.Apply<IntangiblePower>(choiceContext, player, DynamicVars[nameof(CosmicRadiancePower)].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<BurdenPower>(choiceContext, Owner.Creature, DynamicVars[nameof(BurdenPower)].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => DynamicVars[nameof(BurdenPower)].UpgradeValueBy(-1);

}