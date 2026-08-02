using BaseLib.Utils;
using LeagueAncients.Core.Content.Cards;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace LeagueAncients.Core.Content.Relics;

[Pool(typeof(EventRelicPool))]
public class EssenceOfAnAngelMorgana: LeagueAncientsRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips  => HoverTipFactory.FromCardWithCardHoverTips<AchieveDivinityMorgana>();

    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CreateCard<AchieveDivinityMorgana>(Owner), PileType.Deck), 2f);
    }
}