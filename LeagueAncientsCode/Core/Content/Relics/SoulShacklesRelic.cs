using BaseLib.Utils;
using LeagueAncients.Core.Content.Cards;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;

namespace LeagueAncients.Core.Content.Relics;



[Pool(typeof(EventRelicPool))]
public class SoulShacklesRelic : LeagueAncientsRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips  => HoverTipFactory.FromCardWithCardHoverTips<SoulShackles>();

    public override bool HasUponPickupEffect => true;

    private IEnumerable<RelicModel> _relics = null!;
    
    public override async Task AfterObtained()
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CreateCard<SoulShackles>(Owner), PileType.Deck), 2f);
        foreach (var relicModel in _relics)
        {
            await RelicCmd.Obtain(relicModel, Owner);
        }
    }
    
    public void SetRewardRelics(IEnumerable<RelicModel?> relics)
    {
        _relics = relics.OfType<RelicModel>(); // Get rid of possible nulls
    }
    
}