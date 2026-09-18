using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LeagueAncients.Core.Content.Cards;


public class Gem() : LeagueAncientsCardModel(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    // public override string BetaPortraitPath => "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";
    // public override string PortraitPath => "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";
    // public override string CustomPortraitPath => "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new CardsVar(1)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(1, Owner);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}