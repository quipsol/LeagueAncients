using BaseLib.Utils;
using LeagueAncients.Core.Content.Cards;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace LeagueAncients.Core.Content.Relics;

/// <summary>
/// Gain card Celestial Blessing. Heal and Draw.
/// </summary>
[Pool(typeof(EventRelicPool))]
public class DivineFeather : LeagueAncientsRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips  => HoverTipFactory.FromCardWithCardHoverTips<CelestialBlessing>();

    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CreateCard<CelestialBlessing>(Owner), PileType.Deck), 2f);
    }
}