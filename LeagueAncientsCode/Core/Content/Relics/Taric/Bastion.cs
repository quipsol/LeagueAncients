using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;

namespace LeagueAncients.Core.Content.Relics;

/// <summary>
/// Get a card: ALL allies gain Block
/// </summary>
public class Bastion : LeagueAncientsRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    public override bool HasUponPickupEffect => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips  => HoverTipFactory.FromCardWithCardHoverTips<Cards.Bastion>();

    public override async Task AfterObtained()
        => CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CreateCard<Cards.Bastion>(Owner), PileType.Deck), 2f);
}