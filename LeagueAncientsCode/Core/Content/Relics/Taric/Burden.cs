using LeagueAncients.Core.Content.Cards;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LeagueAncients.Core.Content.Relics;


public class Burden : LeagueAncientsRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    public override bool HasUponPickupEffect => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips  => HoverTipFactory.FromCardWithCardHoverTips<Cards.LocketOfTheIronSolari>();

    public override async Task AfterObtained()
        => CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CreateCard<Cards.LocketOfTheIronSolari>(Owner), PileType.Deck), 2f);
}