using BaseLib.Utils;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LeagueAncients.Core.Content.Cards;

[Pool(typeof(ColorlessCardPool))]
public class SorakasCompassion() : LeagueAncientsCardModel(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    public override string BetaPortraitPath => "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";
    public override string PortraitPath => "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";
    public override string CustomPortraitPath => "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
                new DynamicVar("HpVar", 2)
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal, CardKeyword.Exhaust];
    

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars["HpVar"].BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["HpVar"].UpgradeValueBy(3);
    }
}