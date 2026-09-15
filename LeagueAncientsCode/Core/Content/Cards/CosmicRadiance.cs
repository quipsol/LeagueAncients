using LeagueAncients.Core.Content.Powers;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace LeagueAncients.Core.Content.Cards;


public class CosmicRadiance() : LeagueAncientsCardModel(3, CardType.Skill, CardRarity.Ancient, TargetType.AllAllies)
{
    // public override string BetaPortraitPath => "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";
    // public override string PortraitPath => null!;// "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";
    // public override string? CustomPortraitPath => null;// "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CosmicRadiancePower>(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<IntangiblePower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<CosmicRadiancePower>(choiceContext, Owner.Creature, DynamicVars[nameof(CosmicRadiancePower)].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
    
}