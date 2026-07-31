using BaseLib.Utils;
using LeagueAncients.Core.Content.Cards;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace LeagueAncients.Core.Content.Relics;


[Pool(typeof(EventRelicPool))]
public class GuidingLight : LeagueAncientsRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;


    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal currentCost, out decimal modifiedCost)
    {
        if (card.Type is CardType.Attack)
        {
            modifiedCost =  currentCost - 1;
            return true;
        }
        modifiedCost = currentCost;
        return false;
    }
}