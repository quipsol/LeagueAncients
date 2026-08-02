using BaseLib.Utils;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace LeagueAncients.Core.Content.Cards;


[Pool(typeof(CurseCardPool))]
public class SoulShackles() : LeagueAncientsCardModel(-1, CardType.Curse, CardRarity.Ancient, TargetType.Self)
{
    public override string BetaPortraitPath => "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";
    public override string PortraitPath => "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";
    public override string CustomPortraitPath => "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";

    public override int MaxUpgradeLevel => 0;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable, CardKeyword.Eternal];
    
    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != this) return;
        await Cmd.Wait(0.25f); 
        await PlayerCmd.LoseEnergy(DynamicVars.Energy.IntValue, Owner);
    }
    
}